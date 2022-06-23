using System.Security.Claims;

namespace Eiromplays.IdentityServer.Application.Common.Interfaces;

public interface ICurrentUser
{
    string? Name { get; }

    string GetUserId();

    string GetSubjectId();

    string GetDisplayName();

    string GetIdentityProvider();

    string? GetUserEmail();

    string? GetTenant();

    bool IsAuthenticated();

    bool IsInRole(string role);

    IEnumerable<Claim>? GetUserClaims();
}