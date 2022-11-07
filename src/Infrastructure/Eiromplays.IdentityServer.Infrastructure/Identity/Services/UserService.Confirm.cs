using System.Text;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Mailing;
using Eiromplays.IdentityServer.Application.Common.Sms;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Infrastructure.Common;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.Infrastructure.Identity.Models;
using LanguageExt.Common;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class UserService
{
    private async Task<string> GetEmailVerificationUriAsync(ApplicationUser user, string origin)
    {
        string? code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var endpointUri = new Uri(string.Concat(origin, "api/v1/account/confirm-email"));
        string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), QueryStringKeys.UserId, user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Code, code);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.ReturnUrl, new Uri(string.Concat(_spaConfiguration.IdentityServerUiBaseUrl, "auth/confirmed-email")).ToString());

        return verificationUri;
    }

    private async Task<string> SendEmailVerificationAsync(ApplicationUser user, string origin)
    {
        // send verification email
        string emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);

        var emailModel = new RegisterUserEmailModel
        {
            Email = user.Email,
            UserName = user.UserName,
            Url = emailVerificationUri
        };

        var mailRequest = new MailRequest(
            new List<string> { user.Email },
            _t["Confirm Registration"],
            await _templateService.GenerateEmailTemplateAsync("email-confirmation", emailModel));

        _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));

        return _t[$"Please check {user.Email} to verify your account!"];
    }

    public async Task<ResendEmailVerificationResponse> ResendEmailVerificationAsync(ResendEmailVerificationRequest request, string origin)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        return user is null
            ? throw new InternalServerException(_t["An error occurred while trying to resend email verification."])
            : new ResendEmailVerificationResponse { Message = await SendEmailVerificationAsync(user, origin) };
    }

    private async Task<string> SendPhoneNumberVerificationAsync(ApplicationUser user, string? newPhoneNumber = null)
    {
        string? phoneVerificationCode = await _userManager.GenerateChangePhoneNumberTokenAsync(user, newPhoneNumber ?? user.PhoneNumber);

        var smsRequest = new SmsRequest(
            new List<string> { newPhoneNumber ?? user.PhoneNumber },
            _t[$"Please confirm your account by entering this code: {phoneVerificationCode}"]);

        _jobService.Enqueue(() => _smsService.SendAsync(smsRequest, CancellationToken.None));

        return _t["A verification code has been sent to your phone number."];
    }

    public async Task<ResendPhoneNumberVerificationResponse> ResendPhoneNumberVerificationAsync(ResendPhoneNumberVerificationRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(
            x => x.PhoneNumber.Equals(request.PhoneNumber), cancellationToken);

        return user is null
            ? throw new InternalServerException(_t["An error occurred while trying to resend phone number verification."])
            : new ResendPhoneNumberVerificationResponse
            {
                Message = await SendPhoneNumberVerificationAsync(user), ReturnUrl =
                $"{_spaConfiguration.IdentityServerUiBaseUrl}auth/verify-phone-number?userId={user.Id}"
            };
    }

    public async Task<ConfirmEmailResponse> ConfirmEmailAsync(ConfirmEmailRequest request, string origin, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Where(u => u.Id == request.UserId && !u.EmailConfirmed)
            .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new InternalServerException(_t["An error occurred while confirming E-Mail."]);

        var response = new ConfirmEmailResponse();
        var messages = new List<string>();

        request.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
        var result = await _userManager.ConfirmEmailAsync(user, request.Code);

        response.ReturnUrl = request.ReturnUrl;
        if (result.Succeeded && _signInManager.Options.SignIn.RequireConfirmedPhoneNumber &&
            !string.IsNullOrWhiteSpace(user.PhoneNumber))
        {
            string phoneNumberVerificationMessage = await SendPhoneNumberVerificationAsync(user);

            if (!string.IsNullOrWhiteSpace(phoneNumberVerificationMessage))
                messages.Add(phoneNumberVerificationMessage);

            response.ReturnUrl =
                $"{_spaConfiguration.IdentityServerUiBaseUrl}auth/verify-phone-number?userId={user.Id}&returnUrl={request.ReturnUrl}";
        }

        if (result.Succeeded)
            messages.Add(string.Format(_t["Account Confirmed for E-Mail {0}."], user.Email));

        response.Message = string.Join(Environment.NewLine, messages);

        return result.Succeeded
            ? response
            : throw new InternalServerException(string.Format(_t["An error occurred while confirming {0}"], user.Email));
    }

    public async Task<ConfirmPhoneNumberResponse> ConfirmPhoneNumberAsync(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new InternalServerException(_t["An error occurred while confirming Mobile Phone."]);

        var response = new ConfirmPhoneNumberResponse();
        var messages = new List<string>();

        var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, code);

        if (result.Succeeded)
            response.ReturnUrl = $"{_spaConfiguration.IdentityServerUiBaseUrl}auth/confirmed-phone-number";

        messages.Add(!user.EmailConfirmed && _signInManager.Options.SignIn.RequireConfirmedEmail
            ? string.Format(
                _t["Account Confirmed for Phone Number {0}. You should confirm your E-mail before continuing."],
                user.PhoneNumber)
            : string.Format(_t["Account Confirmed for Phone Number {0}."], user.PhoneNumber));

        response.Message = string.Join(Environment.NewLine, messages);

        return result.Succeeded
            ? response
            : throw new InternalServerException(string.Format(_t["An error occurred while confirming {0}"], user.PhoneNumber));
    }
}