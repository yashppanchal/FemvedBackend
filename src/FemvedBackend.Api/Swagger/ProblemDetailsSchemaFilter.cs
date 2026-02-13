using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FemvedBackend.Api.Swagger;

public sealed class ProblemDetailsSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(ProblemDetails))
        {
            ApplyProblemDetailsSchema(schema, includeErrors: false);
            return;
        }

        if (context.Type == typeof(ValidationProblemDetails))
        {
            ApplyProblemDetailsSchema(schema, includeErrors: true);
        }
    }

    private static void ApplyProblemDetailsSchema(OpenApiSchema schema, bool includeErrors)
    {
        schema.Type = "object";
        schema.Properties = new Dictionary<string, OpenApiSchema>
        {
            ["type"] = new OpenApiSchema { Type = "string", Nullable = true },
            ["title"] = new OpenApiSchema { Type = "string", Nullable = true },
            ["status"] = new OpenApiSchema { Type = "integer", Format = "int32", Nullable = true },
            ["detail"] = new OpenApiSchema { Type = "string", Nullable = true },
            ["instance"] = new OpenApiSchema { Type = "string", Nullable = true }
        };

        if (includeErrors)
        {
            schema.Properties["errors"] = new OpenApiSchema
            {
                Type = "object",
                AdditionalProperties = new OpenApiSchema
                {
                    Type = "array",
                    Items = new OpenApiSchema { Type = "string" }
                }
            };
        }

        schema.AdditionalPropertiesAllowed = true;
        schema.AdditionalProperties = new OpenApiSchema { Type = "object" };
    }
}
