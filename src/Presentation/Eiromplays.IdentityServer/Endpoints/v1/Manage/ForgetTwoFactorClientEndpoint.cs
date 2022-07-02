using System.Net;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class ForgetTwoFactorClientEndpoint : EndpointWithoutRequest
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public ForgetTwoFactorClientEndpoint(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public override void Configure()
    {
        Post("/manage/two-factor-authentication/forget");
        Summary(s =>
        {
            s.Summary = "Forget two factor authentication";
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

        await _signInManager.ForgetTwoFactorClientAsync();

        await SendNoContentAsync(cancellation: ct);
    }
}