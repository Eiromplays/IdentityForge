using System.Net;
using System.Text.Encodings.Web;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class EnableAuthenticatorEndpoint : Endpoint<EnableAuthenticator, List<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly UrlEncoder _urlEncoder;
    
    private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

    public EnableAuthenticatorEndpoint(UserManager<ApplicationUser> userManager, UrlEncoder urlEncoder)
    {
        _userManager = userManager;
        _urlEncoder = urlEncoder;
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

    public override async Task HandleAsync(EnableAuthenticator req, CancellationToken ct)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            AddError("User not found");
            await SendErrorsAsync((int)HttpStatusCode.NotFound, ct);
            return;
        }

        var verificationCode = req.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2FaTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2FaTokenValid)
            {
                await LoadSharedKeyAndQrCodeUriAsync(user, req);
                AddError("Verification code is invalid");
                await SendErrorsAsync((int)HttpStatusCode.BadRequest, ct);
                return;
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);

            if (await _userManager.CountRecoveryCodesAsync(user) != 0)
            {
                await SendNoContentAsync(ct);
                return;
            }
        
            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

            await SendOkAsync(recoveryCodes.ToList(), ct);
    }

    private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user, EnableAuthenticator response)
    {
        var sharedKey = await _userManager.GetAuthenticatorKeyAsync(user);
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
            AuthenticatorUriFormat,
            _urlEncoder.Encode("Eiromplays.IdentityServer.Admin"),
            _urlEncoder.Encode(email),
            unformattedKey);
    }
}