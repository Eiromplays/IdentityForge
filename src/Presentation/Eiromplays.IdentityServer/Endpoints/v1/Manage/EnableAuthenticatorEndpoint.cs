using Eiromplays.IdentityServer.Application.Identity.Auth.Requests;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class EnableAuthenticatorEndpoint : Endpoint<EnableAuthenticatorRequest, EnableAuthenticatorResponse>
{
    private readonly IUserService _userService;

    public EnableAuthenticatorEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/manage/two-factor-authentication/enable");
        Summary(s =>
        {
            s.Summary = "Enable two factor authentication";
        });
        Version(1);
    }

    public override async Task HandleAsync(EnableAuthenticatorRequest req, CancellationToken ct)
    {
        await SendOkAsync(await _userService.EnableTwoFactorAsync(req, User), ct);
    }
}