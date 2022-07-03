using System.Text;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Infrastructure.Common;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class UserService
{
    private async Task<string> GetEmailVerificationUriAsync(ApplicationUser user, string origin)
    {
        string? code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        Console.WriteLine($"Code {code}");
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var endpointUri = new Uri(string.Concat(origin, "api/v1/account/confirm-email"));
        string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), QueryStringKeys.UserId, user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Code, code);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.ReturnUrl, new Uri(string.Concat(_spaConfiguration.IdentityServerUiBaseUrl, "auth/confirmed-email")).ToString());

        return verificationUri;
    }

    private async Task<(string VerificationUri, string Code)> GetPhoneNumberVerificationUriAsync(ApplicationUser user, string phoneNumber, string origin)
    {
        string? code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);

        string encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var endpointUri = new Uri(string.Concat(origin, "api/v1/account/confirm-phone-number"));
        string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), QueryStringKeys.UserId, user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Code, encodedCode);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.ReturnUrl, new Uri(string.Concat(_spaConfiguration.IdentityServerUiBaseUrl, "auth/confirmed-phone-number")).ToString());

        return (verificationUri, code);
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
            string phoneNumberVerificationMessage = await SendPhoneNumberVerificationAsync(user, origin);

            if (!string.IsNullOrWhiteSpace(phoneNumberVerificationMessage))
                messages.Add(phoneNumberVerificationMessage);

            response.PhoneNumberVerificationSent = true;
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

    public async Task<string> ConfirmPhoneNumberAsync(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new InternalServerException(_t["An error occurred while confirming Mobile Phone."]);

        var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, code);

        return result.Succeeded
            ? user.EmailConfirmed && !_signInManager.Options.SignIn.RequireConfirmedEmail
                ? string.Format(_t["Account Confirmed for Phone Number {0}."], user.PhoneNumber)
                : string.Format(_t["Account Confirmed for Phone Number {0}. You should confirm your E-mail before continuing."], user.PhoneNumber)
            : throw new InternalServerException(string.Format(_t["An error occurred while confirming {0}"], user.PhoneNumber));
    }
}