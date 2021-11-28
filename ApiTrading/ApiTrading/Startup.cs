namespace ApiTrading
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.Json.Serialization;
    using APIhandler;
    using Configuration;
    using DbContext;
    using Helper;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Modele.DTO.Response;
    using Repository.Signal;
    using Repository.Token;
    using Repository.Utilisateurs;
    using Service.ExternalAPIHandler;
    using Service.Mail;
    using Service.Strategy;
    using Service.Utilisateur;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(x => { x.Filters.Add(new ValidateModelAttribute()); }).ConfigureApiBehaviorOptions(
                options =>
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errorModel = new ErrorModel();
                        var test = context.ModelState.Keys.ToList();
                        var tmp = new List<string>();
                        foreach (var s1 in test) tmp.Add($"Le champ {s1} est invalide");
                        return new BadRequestObjectResult(new
                        {
                            Code = 400,
                            Messages = tmp
                        });
                    }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiTrading", Version = "v1" });
                c.SchemaFilter<SchemaFilter>();
                c.ParameterFilter<SwaggerParameterFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero
            };
            services.AddSingleton(tokenValidationParameters);
            services.AddDbContext<ApiTradingDatabaseContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("ApiTradingDb")));
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));
            services.AddScoped<ValidateModelAttribute>();
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwt =>
                {
                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = tokenValidationParameters;

                    jwt.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = ctx =>
                        {
                            ctx.Response.StatusCode = 401;
                            var errormodel = new BaseResponse("Echec d'authentification");
                            return ctx.Response.WriteAsJsonAsync(errormodel);
                        }
                    };
                });

            services.AddIdentity<IdentityUser<int>, IdentityRole<int>>(options => { })
                .AddEntityFrameworkStores<ApiTradingDatabaseContext>()
                .AddPasswordValidator<PasswordValidatorHelper<IdentityUser<int>>>();

            services.AddHttpContextAccessor();
            services.AddScoped<IMail, MailService>();
            services.AddScoped<IUtilisateurService, UtilisateurService>();
            services.AddScoped<IStrategyService, StrategyService>();
            services.AddSingleton<IApiHandler, XtbApiHandler>();
            services.AddTransient<ISignalRepository, SignalRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            UserManager<IdentityUser<int>> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiTrading v1"); });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            IdentityDataInitializer.SeedData(userManager, roleManager);
            app.UseRouting();

            app.UseAuthorization();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}