using System.Net;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class GenerateRecoveryCodesEndpoint : EndpointWithoutRequest<List<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GenerateRecoveryCodesEndpoint(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Post("/manage/two-factor-authentication/generate-recovery-codes");
        Summary(s =>
        {
            s.Summary = "Generate recovery codes";
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

        if (!user.TwoFactorEnabled)
        {
            ThrowError("Two factor authentication is not enabled");
            return;
        }

        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

        await SendOkAsync(recoveryCodes.ToList(), ct);
    }
}