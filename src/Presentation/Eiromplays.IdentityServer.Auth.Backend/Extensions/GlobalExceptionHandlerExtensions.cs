using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Eiromplays.IdentityServer.Auth.Backend.Extensions;

[ExcludeFromCodeCoverage]
public static class GlobalExceptionHandlerExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError => appError.Run(async httpContext =>
        {
            var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

            if (contextFeature is not null)
            {
                // Set the Http Status Code
                var statusCode = contextFeature.Error switch
                {
                    ValidationException ex => HttpStatusCode.BadRequest,
                    NotFoundException ex => HttpStatusCode.NotFound,
                    UnauthorizedAccessException ex => HttpStatusCode.Unauthorized,
                    ForbiddenAccessException ex => HttpStatusCode.Forbidden,
                    _ => HttpStatusCode.InternalServerError
                };

                // Prepare Generic Error
                var apiError = new ApiError(contextFeature.Error.Message, contextFeature.Error.InnerException?.Message, contextFeature.Error.StackTrace);

                // Set Response Details
                httpContext.Response.StatusCode = (int)statusCode;
                httpContext.Response.ContentType = "application/json";

                // Return the Serialized Generic Error
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(apiError));
            }
        }));

        return app;
    }
}