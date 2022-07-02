using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.Logins;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class ExternalLoginConfirmationEndpoint : Endpoint<CreateExternalUserRequest, ExternalLoginConfirmationResponse>
{
    private readonly IUserService _userService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IServerUrls _serverUrls;
    private readonly IIdentityServerInteractionService _interaction;

    public ExternalLoginConfirmationEndpoint(IUserService userService, SignInManager<ApplicationUser> signInManager, IIdentityServerInteractionService interaction, IServerUrls serverUrls)
    {
        _userService = userService;
        _signInManager = signInManager;
        _interaction = interaction;
        _serverUrls = serverUrls;
    }

    public override void Configure()
    {
        Post("/account/external-logins/confirmation");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Confirm external login";
        });
        AllowAnonymous();
        ScopedValidator();
    }

    public override async Task HandleAsync(CreateExternalUserRequest req, CancellationToken ct)
    {
        // Get the information about the user from the external login provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ThrowError("Error loading external login information during confirmation.");
            return;
        }

        var createUserResponse = await _userService.CreateExternalAsync(req, BaseURL);

        string addLoginMessage = await _userService.AddLoginAsync(createUserResponse.UserId, info.Adapt<UserLoginInfoDto>());

        // TODO: Check if user should be logged in automatically
        /*if (_signInManager.CanSignInAsync())
        await _signInManager.SignInAsync(user, isPersistent: false);*/

        var response = new ExternalLoginConfirmationResponse
        {
            Message = string.Join(Environment.NewLine, new List<string> { createUserResponse.Message, addLoginMessage })
        };

        string? url = !string.IsNullOrWhiteSpace(req.ReturnUrl) ? Uri.UnescapeDataString(req.ReturnUrl) : null;
        var context = await _interaction.GetAuthorizationContextAsync(url);

        if (context != null && !string.IsNullOrWhiteSpace(url))
        {
            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
            response.ReturnUrl = url;

            await SendOkAsync(response, ct);
            return;
        }

        if (string.IsNullOrEmpty(req.ReturnUrl))
        {
            response.ReturnUrl = _serverUrls.BaseUrl;
        }

        await SendOkAsync(response, ct);
    }
}