using System.Security.Claims;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

public class TwoFactorAuthenticationService : ITwoFactorAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public TwoFactorAuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Result<TwoFactorAuthenticationResponse>> GetTwoFactorAuthenticationAsync(ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
        {
            return new Result<TwoFactorAuthenticationResponse>(new NotFoundException("User not found"));
        }

        var response = new TwoFactorAuthenticationResponse
        {
            HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) is not null,
            Is2FaEnabled = user.TwoFactorEnabled,
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
            IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user)
        };

        return response;
    }
}