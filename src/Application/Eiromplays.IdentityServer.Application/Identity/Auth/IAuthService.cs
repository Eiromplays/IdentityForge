using Microsoft.AspNetCore.Http;

namespace Eiromplays.IdentityServer.Application.Identity.Auth;

public interface IAuthService : ITransientService
{
    Task<GetLogoutResponse> BuildLogoutResponseAsync(string logoutId, bool showLogoutPrompt = true);

    Task<dynamic> LogoutAsync(LogoutRequest request, HttpContext httpContext);
}