using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FemvedBackend.Api.Swagger;

public sealed class GlobalResponsesOperationFilter : IOperationFilter
{
    private static readonly OpenApiSchema ProblemDetailsSchema = new()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.Schema,
            Id = nameof(ProblemDetails)
        }
    };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        AddResponse(operation, StatusCodes.Status400BadRequest, "Bad Request");
        AddResponse(operation, StatusCodes.Status401Unauthorized, "Unauthorized");
        AddResponse(operation, StatusCodes.Status403Forbidden, "Forbidden");
        AddResponse(operation, StatusCodes.Status500InternalServerError, "Internal Server Error");
    }

    private static void AddResponse(OpenApiOperation operation, int statusCode, string description)
    {
        var code = statusCode.ToString(CultureInfo.InvariantCulture);
        if (operation.Responses.ContainsKey(code))
        {
            return;
        }

        operation.Responses[code] = new OpenApiResponse
        {
            Description = description,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/problem+json"] = new OpenApiMediaType
                {
                    Schema = ProblemDetailsSchema
                }
            }
        };
    }
}
