using System.Net;
using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Contracts.v1.Requests.Account;
using Eiromplays.IdentityServer.Contracts.v1.Responses.Account;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class Login2FaEndpoint : Endpoint<Login2FaRequest, LoginResponse>
{
    private readonly IUserResolver<ApplicationUser> _userResolver;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IServerUrls _serverUrls;

    public Login2FaEndpoint(IUserResolver<ApplicationUser> userResolver, SignInManager<ApplicationUser> signInManager,
        IIdentityServerInteractionService interaction, IServerUrls serverUrls)
    {
        _userResolver = userResolver;
        _signInManager = signInManager;
        _interaction = interaction;
        _serverUrls = serverUrls;
    }

    public override void Configure()
    {
        Post("/account/login");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Login with email and password";
        });
    }
    
    public override async Task HandleAsync(Login2FaRequest req, CancellationToken ct)
    {
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user is null)
            throw new InvalidOperationException("Unable to get user");
            
        var authenticatorCode = req.TwoFactorCode?.Replace(" ", string.Empty).Replace("-", string.Empty);

        var result =
            await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, req.RememberMe, req.RememberMachine);

        if (result.Succeeded)
        {
            var url = !string.IsNullOrWhiteSpace(req.ReturnUrl) ? Uri.UnescapeDataString(req.ReturnUrl) : null;
            
            if (_interaction.IsValidReturnUrl(url))
                Response.ValidReturnUrl = url ?? _serverUrls.BaseUrl;

            await SendOkAsync(Response, ct);
            return;
        }

        ThrowError("Invalid authentication code");
    }
}