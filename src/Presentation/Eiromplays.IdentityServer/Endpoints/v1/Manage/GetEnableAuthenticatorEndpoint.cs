using System.Net;
using System.Text.Encodings.Web;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class GetEnableAuthenticatorEndpoint : EndpointWithoutRequest<EnableAuthenticator>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly UrlEncoder _urlEncoder;

    public GetEnableAuthenticatorEndpoint(UserManager<ApplicationUser> userManager, UrlEncoder urlEncoder)
    {
        _userManager = userManager;
        _urlEncoder = urlEncoder;
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
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            AddError("User not found");
            await SendErrorsAsync((int)HttpStatusCode.NotFound, ct);
            return;
        }

        var response = new EnableAuthenticator();
        await LoadSharedKeyAndQrCodeUriAsync(user, response);

        await SendOkAsync(response, ct);
    }

    private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user, EnableAuthenticator response)
    {
        string? sharedKey = await _userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(sharedKey))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            sharedKey = await _userManager.GetAuthenticatorKeyAsync(user);
        }

        response.SharedKey = sharedKey;
        if (!string.IsNullOrWhiteSpace(user.Email))
            response.AuthenticatorUri = GenerateQrCodeUri(user.Email, sharedKey);
    }

    private string GenerateQrCodeUri(string email, string unformattedKey)
    {
        return string.Format(
            EnableAuthenticatorEndpoint.AuthenticatorUriFormat,
            _urlEncoder.Encode("Eiromplays.IdentityServer.Admin"),
            _urlEncoder.Encode(email),
            unformattedKey);
    }
}