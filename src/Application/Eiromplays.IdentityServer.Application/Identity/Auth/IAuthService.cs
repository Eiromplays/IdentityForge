using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.ExternalLogins;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Eiromplays.IdentityServer.Application.Identity.Auth;

public interface IAuthService : ITransientService
{
    Task<GetLogoutResponse> BuildLogoutResponseAsync(string logoutId, bool showLogoutPrompt = true);

    Task<LogoutResponse> LogoutAsync<TEndpoint>(LogoutRequest request, HttpContext httpContext)
        where TEndpoint : IEndpoint;

    Task<LoginResponse> LoginAsync(LoginRequest request, string origin);

    Task<LoginResponse> Login2FaAsync(Login2FaRequest request);

    Task<Send2FaVerificationCodeResponse> Send2FaVerificationCodeAsync(
        Send2FaVerificationCodeRequest request);
    Task<IList<string>> GetValidTwoFactorProvidersAsync();

    Task<AuthenticationProperties> ExternalLoginAsync<TEndpoint>(ExternalLoginRequest request, HttpResponse rsp)
        where TEndpoint : IEndpoint;

    Task<LoginResponse> ExternalLoginCallbackAsync(
        ExternalLoginCallbackRequest request, string origin);

    Task LinkExternalLoginAsync<TEndpoint>(
        LinkExternalLoginRequest request, string userId, HttpResponse rsp)
        where TEndpoint : IEndpoint;

    Task<LoginResponse> LinkExternalLoginCallbackAsync(string userId, HttpContext httpContext);

    Task<SendSmsLoginCodeResponse> SendLoginVerificationCodeAsync(SendSmsLoginCodeRequest request);
}