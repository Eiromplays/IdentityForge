using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Infrastructure.Identity;

public class ApplicationSignInManager : SignInManager<ApplicationUser>
{
    public ApplicationSignInManager(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<ApplicationSignInManager> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<ApplicationUser> confirmation)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }

    private async Task<bool> IsTfaEnabledAsync(ApplicationUser user)
        => UserManager.SupportsUserTwoFactor &&
           await UserManager.GetTwoFactorEnabledAsync(user) &&
           (await UserManager.GetValidTwoFactorProvidersAsync(user)).Count > 0;

    /// <summary>
    /// Attempts to sign in the specified <paramref name="user"/> and <paramref name="token"/> combination
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user to sign in.</param>
    /// <param name="token">The token to attempt to sign in with.</param>
    /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
    /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails.</param>
    /// <returns>The task object representing the asynchronous operation containing the <see name="SignInResult"/>
    /// for the sign-in attempt.</returns>
    public virtual async Task<SignInResult> PhoneNumberSignInAsync(ApplicationUser user, string token, bool isPersistent, bool lockoutOnFailure)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var attempt = await CheckPhoneNumberSignInAsync(user, token, lockoutOnFailure);
        return attempt.Succeeded
            ? await SignInOrTwoFactorAsync(user, isPersistent)
            : attempt;
    }

    /// <summary>
    /// Attempts a phone number sign in for a user.
    /// </summary>
    /// <param name="user">The user to sign in.</param>
    /// <param name="token">The token to attempt to sign in with.</param>
    /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails.</param>
    /// <returns>The task object representing the asynchronous operation containing the <see name="SignInResult"/>
    /// for the sign-in attempt.</returns>
    /// <returns></returns>
    private async Task<SignInResult> CheckPhoneNumberSignInAsync(ApplicationUser user, string token, bool lockoutOnFailure)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var error = await PreSignInCheck(user);
        if (error != null)
        {
            return error;
        }

        if (await UserManager.VerifyChangePhoneNumberTokenAsync(user, token, user.PhoneNumber ?? string.Empty))
        {
            bool alwaysLockout = AppContext.TryGetSwitch("Microsoft.AspNetCore.Identity.CheckPasswordSignInAlwaysResetLockoutOnSuccess", out bool enabled) && enabled;

            // Only reset the lockout when not in quirks mode if either TFA is not enabled or the client is remembered for TFA.
            if (alwaysLockout || !await IsTfaEnabledAsync(user) || await IsTwoFactorClientRememberedAsync(user))
            {
                await ResetLockout(user);
            }

            return SignInResult.Success;
        }

        Logger.LogWarning(new EventId(6, "InvalidToken"), "User failed to provide the correct token");

        if (!UserManager.SupportsUserLockout || !lockoutOnFailure) return SignInResult.Failed;

        // If lockout is requested, increment access failed count which might lock out the user
        await UserManager.AccessFailedAsync(user);
        if (await UserManager.IsLockedOutAsync(user))
        {
            return await LockedOut(user);
        }

        return SignInResult.Failed;
    }
}