using Microsoft.AspNetCore.Http;

namespace Eiromplays.IdentityServer.Application.Identity.Auth;

public interface IAuthService : ITransientService
{
    Task<GetLogoutResponse> BuildLogoutResponseAsync(string logoutId, bool showLogoutPrompt = true);

    Task<dynamic> LogoutAsync(LogoutRequest request, HttpContext httpContext);

    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);

    Task<Result<LoginResponse>> Login2FaAsync(Login2FaRequest request);
}