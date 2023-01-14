using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Mailing;
using Eiromplays.IdentityServer.Application.Identity.Users.Password;
using Microsoft.AspNetCore.WebUtilities;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class UserService
{
    public async Task<string> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
    {
        var user = await _userManager.FindByEmailAsync(request.Email.Normalize());
        if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
        {
            // Don't reveal that the user does not exist or is not confirmed
            throw new InternalServerException(_t["An Error has occurred!"]);
        }

        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        string code = await _userManager.GeneratePasswordResetTokenAsync(user);
        var endpointUri = new Uri(string.Concat(_urlConfiguration.IdentityServerUiBaseUrl, "auth/reset-password"));

        string passwordResetUrl = QueryHelpers.AddQueryString(endpointUri.ToString(), "token", code);

        var mailRequest = new MailRequest(
            new List<string> { request.Email },
            _t["Reset Password"],
            _t[$"Your Password Reset Token is '{code}'. You can reset your password using this link: {passwordResetUrl}"]);
        _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));

        return _t["Password Reset Mail has been sent to your authorized Email."];
    }

    public async Task<string> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email.Normalize());

        // Don't reveal that the user does not exist
        _ = user ?? throw new InternalServerException(_t["An Error has occurred!"]);

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

        return result.Succeeded
            ? _t["Password Reset Successful!"]
            : throw new InternalServerException(_t["An Error has occurred!"]);
    }

    public async Task<string> ChangePasswordAsync(ChangePasswordRequest model, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        if (!await HasPasswordAsync(userId))
            return await SetPasswordAsync(new SetPasswordRequest(model.NewPassword, model.ConfirmNewPassword), userId);

        var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Change password failed"], result.GetErrors(_t));
        }

        return _t["Password Changed Successfully!"];
    }

    public async Task<string> SetPasswordAsync(SetPasswordRequest model, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        if (await HasPasswordAsync(userId))
            throw new BadRequestException(_t["User already has a password."]);

        var addPasswordResult = await _userManager.AddPasswordAsync(user, model.Password);

        if (!addPasswordResult.Succeeded)
        {
            throw new InternalServerException(_t["Set password failed"], addPasswordResult.GetErrors(_t));
        }

        await _signInManager.RefreshSignInAsync(user);

        return _t["Password set."];
    }

    public async Task<bool> HasPasswordAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        return await _userManager.HasPasswordAsync(user);
    }
}