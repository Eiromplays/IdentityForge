using System.Security.Claims;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;

namespace Eiromplays.IdentityServer.Application.Identity.Auth;

public interface ITwoFactorAuthenticationService : ITransientService
{
    Task<Result<TwoFactorAuthenticationResponse>> GetTwoFactorAuthenticationAsync(ClaimsPrincipal claimsPrincipal);
}