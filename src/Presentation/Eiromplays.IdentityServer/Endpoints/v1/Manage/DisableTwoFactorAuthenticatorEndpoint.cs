using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class DisableTwoFactorAuthenticatorEndpoint : EndpointWithoutRequest<DisableTwoFactorAuthenticatorResponse>
{
    private readonly IUserService _userService;

    public DisableTwoFactorAuthenticatorEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/manage/two-factor-authentication/disable");
        Summary(s =>
        {
            s.Summary = "Disable two factor authentication";
        });
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetUserId();

        if (string.IsNullOrWhiteSpace(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        Response.Message = await _userService.DisableTwoFactorAsync(userId);

        await SendOkAsync(Response, ct);
    }
}