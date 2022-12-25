using System.Security.Claims;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class UserService
{
    public async Task<GetEnableAuthenticatorResponse> GetEnableTwoFactorAsync(string? userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        var response = new GetEnableAuthenticatorResponse();

        await LoadSharedKeyAndQrCodeUriAsync(user, response);

        return response;
    }

    public async Task<EnableAuthenticatorResponse> EnableTwoFactorAsync(EnableAuthenticatorRequest req, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        var validProvider = await _userManager.GetValidTwoFactorProvidersAsync(user);

        string verificationCode = req.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

        bool is2FaTokenValid = validProvider.Contains(req.Provider, StringComparer.OrdinalIgnoreCase) ||
                               await _userManager.VerifyTwoFactorTokenAsync(
                                   user, req.Provider, verificationCode);

        if (!is2FaTokenValid)
            throw new BadRequestException("Verification code is invalid");

        var response = new EnableAuthenticatorResponse();

        await _userManager.SetTwoFactorEnabledAsync(user, true);

        if (await _userManager.CountRecoveryCodesAsync(user) != 0)
        {
            return response;
        }

        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

        response.RecoveryCodes = recoveryCodes.ToList();

        return response;
    }

    private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user, GetEnableAuthenticatorResponse response)
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
            AuthenticatorUriFormat,
            _urlEncoder.Encode("Eiromplays.IdentityServer.Admin"),
            _urlEncoder.Encode(email),
            unformattedKey);
    }

    public async Task<string> DisableTwoFactorAsync(string? userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        var result = await _userManager.SetTwoFactorEnabledAsync(user, false);

        return result.Succeeded
            ? _t["Two-factor Disabled Successfully!"]
            : throw new InternalServerException(_t["An Error has occurred!", result.GetErrors(_t)]);
    }

    public async Task<TwoFactorAuthenticationResponse> GetTwoFactorAuthenticationAsync(ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        var validTwoFactorProviders = await _userManager.GetValidTwoFactorProvidersAsync(user);

        var response = new TwoFactorAuthenticationResponse
        {
            ValidProviders = validTwoFactorProviders,
            HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) is not null,
            Is2FaEnabled = user.TwoFactorEnabled,
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
            IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user)
        };

        return response;
    }

    public async Task<IList<string>> GetValidTwoFactorProvidersAsync(string? userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new List<string>();

        return await _userManager.GetValidTwoFactorProvidersAsync(user);
    }
}