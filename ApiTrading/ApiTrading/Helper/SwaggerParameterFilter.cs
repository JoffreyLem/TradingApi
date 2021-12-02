using System;
using System.Linq;
using ApiTrading.Service.Strategy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiTrading.Helper
{
    public class SwaggerParameterFilter : IParameterFilter

    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SwaggerParameterFilter(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (parameter.Name.Equals("strategy", StringComparison.InvariantCultureIgnoreCase))
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var serviceStrategy = scope.ServiceProvider.GetRequiredService<IStrategyService>();
                    var allStrategy = serviceStrategy.GetAllStrategy().Result;
                    var data = allStrategy.Data;
                    parameter.Schema.Enum = data.Select(x => new OpenApiString(x.Name)).ToList<IOpenApiAny>();
                }

            if (parameter.Name.Equals("timeframe", StringComparison.InvariantCultureIgnoreCase))
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