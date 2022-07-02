using System.Net;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class ResetAuthenticatorEndpoint : EndpointWithoutRequest
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ResetAuthenticatorEndpoint(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Post("/manage/two-factor-authentication/reset");
        Summary(s =>
        {
            s.Summary = "Reset the authenticator";
        });
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            AddError("User not found");
            await SendErrorsAsync((int)HttpStatusCode.NotFound, ct);
            return;
        }

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);

        await SendNoContentAsync(ct);
    }
}