using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using APIhandler;
using ApiTrading.Configuration;
using ApiTrading.Controllers;
using ApiTrading.DbContext;
using ApiTrading.Exception;
using ApiTrading.Service.ExternalAPIHandler;
using ApiTrading.Service.Mail;
using ApiTrading.Service.Strategy;
using ApiTrading.Service.Utilisateur;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


namespace ApiTrading
{
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
            services.AddControllers(x =>
            {
                x.Filters.Add(new ValidateModelAttribute());
            }).ConfigureApiBehaviorOptions(options =>
                
                
                options.InvalidModelStateResponseFactory= context =>
                {
                    var errorModel = new ErrorModel();
                    errorModel.StatusCode = 400;
                    errorModel.ErrorMessage = context.ModelState.Values.SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage).ToList();
                    return new BadRequestObjectResult(new {
                        Code = 400,
                        Messages = context.ModelState.Values.SelectMany(x => x.Errors)
                            .Select(x => x.ErrorMessage)
                    });
                }  ).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            } );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ApiTrading", Version = "v1"});
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });
            var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

            var tokenValidationParameters = new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,

                // Allow to use seconds for expiration of token
                // Required only when token lifetime less than 5 minutes
                // THIS ONE
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

                    jwt.Events = new JwtBearerEvents()
                    {
                        
                        OnAuthenticationFailed = ctx =>
                        {
                            ctx.Response.StatusCode = 401;
                           ErrorModel errormodel = new ErrorModel(401,"Echec d'authentification");
                           return ctx.Response.WriteAsJsonAsync(errormodel);
                        },
                        
                    };


                });

            services.AddDefaultIdentity<IdentityUser<int>>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApiTradingDatabaseContext>();

            services.AddScoped<IMail, MailService>();
            services.AddScoped<IUtilisateurService, UtilisateurService>();
            services.AddScoped<IStrategyService, StrategyService>();
            services.AddSingleton<IApiHandler, XtbApiHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiTrading v1"));
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}