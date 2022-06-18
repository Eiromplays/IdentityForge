using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class GetLogin2FaEndpoint : Endpoint<GetLogin2FaRequest, GetLogin2FaResponse>
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public GetLogin2FaEndpoint(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public override void Configure()
    {
        Get("/account/login2fa");
        Summary(s =>
        {
            s.Summary = "Get login 2fa";
        });
        Version(1);
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(GetLogin2FaRequest req, CancellationToken ct)
    {
        // Ensure the user has gone through the username & password screen first
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

        if (user is null)
        {
            AddError("Unable to get user");
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        Response = new GetLogin2FaResponse
        {
            ReturnUrl = req.ReturnUrl,
            RememberMe = req.RememberMe
        };

        await SendOkAsync(Response, cancellation: ct);
    }
}