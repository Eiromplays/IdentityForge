using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class GetTwoFactorAuthenticationEndpoint : EndpointWithoutRequest<TwoFactorAuthenticationResponse>
{
    private readonly IUserService _userService;

    public GetTwoFactorAuthenticationEndpoint(IUserService userService)
    {
        _userService = userService;
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
        await this.ResultToResponseAsync(
            await _userService.GetTwoFactorAuthenticationAsync(User),
            cancellationToken: ct);
    }
}