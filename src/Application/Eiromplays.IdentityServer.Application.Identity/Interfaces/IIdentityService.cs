using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.DTOs.User;

namespace Eiromplays.IdentityServer.Application.Identity.Interfaces;

public interface IIdentityService
{
    Task<(Result Result, string? UserId)> CreateUserAsync(UserDto? userDto);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<string?> GetUserNameAsync(string userId);
}