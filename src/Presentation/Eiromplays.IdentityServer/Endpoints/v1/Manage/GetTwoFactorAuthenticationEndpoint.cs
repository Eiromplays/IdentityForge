using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class GetTwoFactorAuthenticationEndpoint : EndpointWithoutRequest<TwoFactorAuthenticationResponse>
{
    private readonly ITwoFactorAuthenticationService _twoFactorAuthenticationService;

    public GetTwoFactorAuthenticationEndpoint(ITwoFactorAuthenticationService twoFactorAuthenticationService)
    {
        _twoFactorAuthenticationService = twoFactorAuthenticationService;
    }

    public override void Configure()
    {
        Get("/manage/two-factor-authentication");
        Summary(s =>
        {
            s.Summary = "Get two factor authentication information for the current user.";
        });
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _twoFactorAuthenticationService.GetTwoFactorAuthenticationAsync(User);
        await result.Match(
            async x =>
        {
            await SendOkAsync(x, cancellation: ct);
        },
            async exception =>
        {
            if (exception is NotFoundException notFoundException)
            {
                AddError(notFoundException.Message);
                await SendErrorsAsync((int)notFoundException.StatusCode, ct);
                return;
            }

            await SendErrorsAsync(cancellation: ct);
        });
    }
}