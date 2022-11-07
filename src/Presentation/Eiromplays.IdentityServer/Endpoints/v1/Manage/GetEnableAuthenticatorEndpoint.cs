using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class GetEnableAuthenticatorEndpoint : EndpointWithoutRequest<GetEnableAuthenticatorResponse>
{
    private readonly IUserService _userService;

    public GetEnableAuthenticatorEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/manage/two-factor-authentication/enable");
        Summary(s =>
        {
            s.Summary = "Enable two factor authentication";
        });
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await this.ResultToResponseAsync(
            await _userService.GetEnableTwoFactorAsync(User.GetUserId() ?? string.Empty),
            cancellationToken: ct);
    }
}