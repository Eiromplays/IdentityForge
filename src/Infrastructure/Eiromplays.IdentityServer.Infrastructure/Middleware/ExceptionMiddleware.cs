using System.Net;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Serilog;
using Serilog.Context;

namespace Eiromplays.IdentityServer.Infrastructure.Middleware;

internal class ExceptionMiddleware : IMiddleware
{
    private readonly ICurrentUser _currentUser;
    private readonly IStringLocalizer _t;
    private readonly ISerializerService _jsonSerializer;

    public ExceptionMiddleware(
        ICurrentUser currentUser,
        IStringLocalizer<ExceptionMiddleware> localizer,
        ISerializerService jsonSerializer)
    {
        _currentUser = currentUser;
        _t = localizer;
        _jsonSerializer = jsonSerializer;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var email = _currentUser.GetUserEmail() is { } userEmail ? userEmail : "Anonymous";
            var userId = _currentUser.GetUserId();
            var tenant = _currentUser.GetTenant() ?? string.Empty;
            
            if (!string.IsNullOrWhiteSpace(userId)) LogContext.PushProperty("UserId", userId);
            LogContext.PushProperty("UserEmail", email);
            
            if (!string.IsNullOrEmpty(tenant)) LogContext.PushProperty("Tenant", tenant);
            
            var errorId = Guid.NewGuid().ToString();
            
            LogContext.PushProperty("ErrorId", errorId);
            LogContext.PushProperty("StackTrace", exception.StackTrace);

            var errorResult = new ErrorResult
            {
                Source = exception.TargetSite?.DeclaringType?.FullName,
                Exception = exception.Message.Trim(),
                ErrorId = errorId,
                SupportMessage = _t["Provide the ErrorId {0} to the support team for further analysis.", errorId]
            };
            
            errorResult.Messages.Add(exception.Message);
            
            if (exception is not CustomException && exception.InnerException is not null)
            {
                while (exception.InnerException is not null)
                {
                    exception = exception.InnerException;
                }
            }

            switch (exception)
            {
                case CustomException e:
                    errorResult.StatusCode = (int)e.StatusCode;
                    if (e.ErrorMessages is not null)
                    {
                        errorResult.Messages = e.ErrorMessages;
                    }

                    break;

                case KeyNotFoundException:
                    errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            Log.Error(
                "{Exception} Request failed with Status Code {StatusCode} and Error Id {ErrorId}",
                errorResult.Exception, context.Response.StatusCode, errorId);
            
            var response = context.Response;
            
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = errorResult.StatusCode;
                
                await response.WriteAsync(_jsonSerializer.Serialize(errorResult));
            }
            else
            {
                Log.Warning("Can't write error response. Response has already started");
            }
        }
    }
}