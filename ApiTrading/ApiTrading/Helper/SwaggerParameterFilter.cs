namespace ApiTrading.Helper
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Service.Strategy;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class SwaggerParameterFilter : IParameterFilter

    {
        readonly IServiceScopeFactory _serviceScopeFactory;  
        
        public SwaggerParameterFilter(IServiceScopeFactory serviceScopeFactory)  
        {  
            _serviceScopeFactory = serviceScopeFactory;  
        }
        
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (parameter.Name.Equals("strategy", StringComparison.InvariantCultureIgnoreCase))
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var serviceStrategy = scope.ServiceProvider.GetRequiredService<IStrategyService>();
                    var allStrategy = serviceStrategy.GetAllStrategy().Result;
                    var data = allStrategy.Data;
                    parameter.Schema.Enum = data.Select(x => new OpenApiString(x.Name)).ToList<IOpenApiAny>();

                }
            }
            
            if (parameter.Name.Equals("timeframe", StringComparison.InvariantCultureIgnoreCase))
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var serviceStrategy = scope.ServiceProvider.GetRequiredService<IStrategyService>();
                    var allStrategy = serviceStrategy.GetAllTimeframe().Result;
                    var data = allStrategy.Data;
                    parameter.Schema.Enum = data.Select(x => new OpenApiString(x)).ToList<IOpenApiAny>();

                }
            }
        }
    }
}