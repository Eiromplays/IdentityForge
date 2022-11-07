using FluentValidation.Results;

namespace Eiromplays.IdentityServer.Application.Common.Extensions;

public static class FastEndpointsExtensions
{
    public static async Task ResultToResponseAsync<TResponse>(
        this IEndpoint? endpoint,
        Result<TResponse> result,
        Func<TResponse, Task<bool>>? customFunc = null,
        CancellationToken cancellationToken = default)
        where TResponse : notnull
    {
        if (endpoint is null) return;

        var ctx = endpoint.HttpContext;
        await result.Match(
            async x =>
            {
                if (ctx.Response.HasStarted)
                {
                    Console.WriteLine($"Response has already started for {ctx.Request.Path}");
                    return;
                }

                if (customFunc is not null)
                {
                    if (await customFunc(x).ConfigureAwait(false))
                        return;
                }

                await ctx.Response.SendOkAsync(x, cancellation: cancellationToken).ConfigureAwait(false);
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
                        await ctx.Response.SendErrorsAsync(
                            new List<ValidationFailure>
                        {
                            new("BadRequest", badRequestException.Message)
                        },
                            cancellation: cancellationToken).ConfigureAwait(false);
                        return;
                    case InternalServerException internalServerException:
                        await ctx.Response.SendErrorsAsync(
                            new List<ValidationFailure>
                        {
                            new("InternalServerException", internalServerException.Message)
                        },
                            statusCode: (int)internalServerException.StatusCode,
                            cancellation: cancellationToken).ConfigureAwait(false);
                        return;
                    case NotFoundException notFoundException:
                        await ctx.Response.SendErrorsAsync(
                            new List<ValidationFailure>
                        {
                            new("NotFoundException", notFoundException.Message)
                        },
                            statusCode: (int)notFoundException.StatusCode,
                            cancellation: cancellationToken).ConfigureAwait(false);
                        return;
                    case ForbiddenException forbiddenException:
                        await ctx.Response.SendErrorsAsync(
                            new List<ValidationFailure>
                        {
                            new("ForbiddenException", forbiddenException.Message)
                        },
                            statusCode: (int)forbiddenException.StatusCode,
                            cancellation: cancellationToken).ConfigureAwait(false);
                        return;
                    case UnauthorizedException unauthorizedException:
                        await ctx.Response.SendErrorsAsync(
                            new List<ValidationFailure>
                        {
                            new("UnauthorizedException", unauthorizedException.Message)
                        },
                            statusCode: (int)unauthorizedException.StatusCode,
                            cancellation: cancellationToken).ConfigureAwait(false);
                        return;
                    case ConflictException conflictException:
                        await ctx.Response.SendErrorsAsync(
                            new List<ValidationFailure>
                        {
                            new("ConflictException", conflictException.Message)
                        },
                            statusCode: (int)conflictException.StatusCode,
                            cancellation: cancellationToken).ConfigureAwait(false);
                        return;
                }

                await ctx.Response.SendErrorsAsync(new List<ValidationFailure>(), cancellation: cancellationToken).ConfigureAwait(false);
            }).ConfigureAwait(false);
    }
}