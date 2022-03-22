using System.Security.Claims;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Eiromplays.IdentityServer.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue("sub");
}