using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.ExternalLogins;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Eiromplays.IdentityServer.Application.Identity.Auth;

public interface IAuthService : ITransientService
{
    Task<GetLogoutResponse> BuildLogoutResponseAsync(string logoutId, bool showLogoutPrompt = true);

    Task<LogoutResponse> LogoutAsync<TEndpoint>(LogoutRequest request, HttpContext httpContext)
        where TEndpoint : IEndpoint;

    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);

    Task<Result<LoginResponse>> Login2FaAsync(Login2FaRequest request);

    Task<Result<Send2FaVerificationCodeResponse>> Send2FaVerificationCodeAsync(
        Send2FaVerificationCodeRequest request);
    Task<Result<IList<string>>> GetValidTwoFactorProvidersAsync();

    Task<Result<AuthenticationProperties>> ExternalLoginAsync<TEndpoint>(ExternalLoginRequest request, HttpResponse rsp)
        where TEndpoint : IEndpoint;

    Task<Result<LoginResponse>> ExternalLoginCallbackAsync(
        ExternalLoginCallbackRequest request);

    Task LinkExternalLoginAsync<TEndpoint>(
        LinkExternalLoginRequest request, string userId, HttpResponse rsp)
        where TEndpoint : IEndpoint;

    Task<Result<LoginResponse>> LinkExternalLoginCallbackAsync(string userId, HttpContext httpContext);

    Task<Result<SendSmsLoginCodeResponse>> SendLoginVerificationCodeAsync(SendSmsLoginCodeRequest request);
}