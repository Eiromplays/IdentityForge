using FluentValidation.Results;

namespace Eiromplays.IdentityServer.Application.Common.Extensions;

public static class FastEndpointsExtensions
{
    public static async Task ResultToResponseAsync<TResponse>(
        this IEndpoint endpoint,
        Result<TResponse> result,
        CancellationToken ct = default,
        string redirectPropertyName = "",
        bool permanentRedirect = true,
        Func<TResponse, Task<bool>>? customFunc = null)
        where TResponse : notnull
    {
        var ctx = endpoint.HttpContext;
        await result.Match(
            async x =>
            {
                if (ctx.Response.HasStarted)
                {
                    Console.WriteLine($"Response has already started for {ctx.Request.Path}");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(redirectPropertyName))
                {
                    string redirectUrl = x.GetType().GetProperty(redirectPropertyName)?.GetValue(x)?.ToString() ??
                                         string.Empty;

                    if (!string.IsNullOrWhiteSpace(redirectUrl))
                    {
                        await ctx.Response.SendRedirectAsync(redirectUrl, permanentRedirect, ct);
                        return;
                    }
                }

                if (customFunc is not null)
                {
                    if (await customFunc(x))
                        return;
                }

                await ctx.Response.SendOkAsync(x, cancellation: ct);
            },
            async exception =>
            {
                if (ctx.Response.HasStarted)
                {
                    Console.WriteLine($"Something went wrong: {exception.Message}");
                    return;
                }

                switch (exception)
                {
                    case BadRequestException badRequestException:
                        await ctx.Response.SendErrorsAsync(new List<ValidationFailure>
                        {
                            new("BadRequest", badRequestException.Message)
                        }, cancellation: ct);
                        return;
                    case InternalServerException internalServerException:
                        await ctx.Response.SendErrorsAsync(new List<ValidationFailure>
                        {
                            new("InternalServerException", internalServerException.Message)
                        }, statusCode: (int)internalServerException.StatusCode, cancellation: ct);
                        return;
                    case NotFoundException notFoundException:
                        await ctx.Response.SendErrorsAsync(new List<ValidationFailure>
                        {
                            new("NotFoundException", notFoundException.Message)
                        }, statusCode: (int)notFoundException.StatusCode, cancellation: ct);
                        return;
                    case ForbiddenException forbiddenException:
                        await ctx.Response.SendErrorsAsync(new List<ValidationFailure>
                        {
                            new("ForbiddenException", forbiddenException.Message)
                        }, statusCode: (int)forbiddenException.StatusCode, cancellation: ct);
                        return;
                    case UnauthorizedException unauthorizedException:
                        await ctx.Response.SendErrorsAsync(new List<ValidationFailure>
                        {
                            new("UnauthorizedException", unauthorizedException.Message)
                        }, statusCode: (int)unauthorizedException.StatusCode, cancellation: ct);
                        return;
                    case ConflictException conflictException:
                        await ctx.Response.SendErrorsAsync(new List<ValidationFailure>
                        {
                            new("ConflictException", conflictException.Message)
                        }, statusCode: (int)conflictException.StatusCode, cancellation: ct);
                        return;
                }

                await ctx.Response.SendErrorsAsync(new List<ValidationFailure>(), cancellation: ct);
            });
    }
}