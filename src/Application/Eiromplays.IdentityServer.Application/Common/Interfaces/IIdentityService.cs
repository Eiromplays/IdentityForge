using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.DTOs.Role;
using Eiromplays.IdentityServer.Application.DTOs.User;

namespace Eiromplays.IdentityServer.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(Result Result, string? UserId)> CreateUserAsync(UserDto? userDto);
    Task<bool> IsInRoleAsync(string userId, string role);
    Task<bool> CanSignInAsync(string userId);
    Task<bool> AuthorizeAsync(string userId, string policyName);
    Task<(Result Result, string? UserId)> UpdateUserAsync(UserDto userDto);
    Task<bool> UserExistsAsync(string? userId);
    Task<UserDto?> FindUserByIdAsync(string? userId);
    Task<UserDto?> FindUserByUsernameAsync(string? username);
    Task<string?> GetUserNameAsync(string? userId);
    Task<string?> GetDisplayNameAsync(string? userId);
    Task<PaginatedList<UserDto>> GetUsersAsync(string? search, int? pageIndex, int? pageSize, CancellationToken cancellationToken = default);

    Task<PaginatedList<UserDto>> GetRoleUsersAsync(string? roleName, string? search,
        int pageIndex = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    Task<PaginatedList<UserDto>> GetClaimUsersAsync(string? claimType, string? claimValue,
        int pageIndex = 1, int pageSize = 10);

    Task<PaginatedList<UserClaimDto>> GetUserClaimsAsync(string? userId, int pageIndex = 1,
        int pageSize = 10);

    Task<PaginatedList<RoleClaimDto>> GetUserRoleClaimsAsync(string? userId,
        string? claimSearchText, int pageIndex = 1, int pageSize = 10);

    Task<UserClaimDto?> GetUserClaimAsync(string? userId, int claimId);
    Task<UserClaimDto?> GetUserClaimAsync(string? userId, string? claimType, string? claimValue);
    Task<Result?> CreateUserClaimAsync(UserClaimDto userClaimDto);
    Task<Result> UpdateUserClaimAsync(UserClaimDto newUserClaimDto);
    Task<Result> DeleteUserClaimAsync(string? userId, int claimId);
    Task<UserClaimDto?> GetUserClaimAsync(int claimId);
    Task<IReadOnlyList<UserLoginDto>> GetUserProvidersAsync(string? userId);
    Task<UserLoginDto?> GetUserProviderAsync(string? userId, string? providerKey);
    Task<UserLoginDto?> GetUserProviderAsync(string? userId, string? providerKey, string? loginProvider);
    Task<Result> DeleteUserProviderAsync(string? userId, string? providerKey, string? loginProvider);
    Task<Result> ChangeUserPasswordAsync(string? userId, string? password);
    Task<Result> DeleteUserAsync(string? userId);

    Task<Result> AddUserToRolesAsync(string? userId, IEnumerable<string> roles);

    Task<Result> AddUserToRoleAsync(string? userId, string role);

    Task<RoleDto?> FindRoleByIdAsync(string? roleId);
    Task<bool> RoleExistsAsync(string? roleId);
    Task<IReadOnlyList<RoleDto>> GetRolesAsync();

    Task<PaginatedList<RoleDto>> GetRolesAsync(string? search, int? pageIndex ,
        int? pageSize);

    Task<PaginatedList<RoleDto>> GetUserRolesAsync(string? userId, int pageIndex = 1,
        int pageSize = 10);

    Task<(Result Result, string? RoleId)> CreateRoleAsync(RoleDto roleDto);
    Task<(Result Result, string? RoleId)> UpdateRoleAsync(RoleDto roleDto);
    Task<(Result Result, string? RoleId)> DeleteRoleAsync(RoleDto roleDto);

    Task<PaginatedList<RoleClaimDto>> GetRoleClaimsAsync(string? roleId, int pageIndex = 1,
        int pageSize = 10);

    Task<RoleClaimDto?> GetRoleClaimAsync(string? roleId, int claimId);
    Task<Result> CreateRoleClaimAsync(RoleClaimDto roleClaim);
    Task<Result> UpdateRoleClaimAsync(RoleClaimDto newRoleClaim);
    Task<Result> DeleteRoleClaimAsync(string? roleId, int claimId);
}