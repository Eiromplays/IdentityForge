using System.Net;
using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Contracts.v1.Requests.Account;
using Eiromplays.IdentityServer.Contracts.v1.Responses.Account;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    private readonly IUserResolver<ApplicationUser> _userResolver;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IServerUrls _serverUrls;

    public LoginEndpoint(IUserResolver<ApplicationUser> userResolver, SignInManager<ApplicationUser> signInManager,
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
    
    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var user = await _userResolver.GetUserAsync(req.Login);
        if (user is null)
        {
            ThrowError("Invalid username or password");
            return;
        }
        
        var loginResult = await _signInManager.PasswordSignInAsync(user, req.Password, req.RememberMe, lockoutOnFailure: true);
        Response.SignInResult = loginResult;

        if (!loginResult.Succeeded)
        {
            if (loginResult.RequiresTwoFactor)
            {
                await SendRedirectAsync($"account/login/2fa?rememberMe={req.RememberMe}&returnUrl={req.ReturnUrl}", cancellation: ct);
                return;
            }

            if (loginResult.IsLockedOut)
            {
                // TODO: Option to send email to user to notify them of their account being locked 
                await SendAsync(Response, (int)HttpStatusCode.BadRequest, ct);
                return;
            }

            Response.Error = "Invalid username or password";
            await SendAsync(Response, (int)HttpStatusCode.BadRequest, ct);
            return;
        }

        var context = await _interaction.GetAuthorizationContextAsync(req.ReturnUrl);

        Response.ValidReturnUrl = context is not null ? req.ReturnUrl : _serverUrls.BaseUrl;

        await SendOkAsync(Response, ct);
    }
}