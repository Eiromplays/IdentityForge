using System.Security.Claims;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Shared.Authorization;
using PrincipalExtensions = Duende.IdentityServer.Extensions.PrincipalExtensions;

namespace Eiromplays.IdentityServer.Infrastructure.Auth;

public class CurrentUser : ICurrentUser, ICurrentUserInitializer
{
    private ClaimsPrincipal? _user;

    public string? Name => _user?.Identity?.Name;

    private string _userId = "";

    public string GetUserId() =>
        IsAuthenticated()
            ? _user?.GetUserId() ?? ""
            : _userId;
    
    public string GetSubjectId() =>
        IsAuthenticated()
            ? PrincipalExtensions.GetSubjectId(_user) ?? ""
            : _userId;
    
    public string GetDisplayName() =>
        IsAuthenticated()
            ?  PrincipalExtensions.GetDisplayName(_user)
            : string.Empty;
    
    public string GetIdentityProvider() =>
        IsAuthenticated()
            ? PrincipalExtensions.GetIdentityProvider(_user)
            : string.Empty;

    public string? GetUserEmail() =>
        IsAuthenticated()
            ? _user?.GetEmail()
            : string.Empty;

    public bool IsAuthenticated() =>
        _user?.Identity?.IsAuthenticated is true;

    public bool IsInRole(string role) =>
        _user?.IsInRole(role) is true;

    public IEnumerable<Claim>? GetUserClaims() =>
        _user?.Claims;

    public string? GetTenant() =>
        IsAuthenticated() ? _user?.GetTenant() : string.Empty;

    public void SetCurrentUser(ClaimsPrincipal user)
    {
        if (_user != null)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        _user = user;
    }

    public void SetCurrentUserId(string userId)
    {
        if (!string.IsNullOrWhiteSpace(_userId))
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        if (!string.IsNullOrEmpty(userId))
        {
            _userId = userId;
        }
    }
}