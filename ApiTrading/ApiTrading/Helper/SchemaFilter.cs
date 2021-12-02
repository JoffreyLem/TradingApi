﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiTrading.Helper
{
    public class SchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null) return;

            foreach (var property in schema.Properties)
                if (property.Value.Default != null && property.Value.Example == null)
                    property.Value.Default = property.Value.Default;
        }
    }
}