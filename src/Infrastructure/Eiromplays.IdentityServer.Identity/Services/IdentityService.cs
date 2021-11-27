using AutoMapper;
using Eiromplays.IdentityServer.Application.Common.Interface;
using Eiromplays.IdentityServer.Application.Common.Mappings;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.DTOs.Role;
using Eiromplays.IdentityServer.Application.Identity.DTOs.User;
using Eiromplays.IdentityServer.Infrastructure.Extensions;
using Eiromplays.IdentityServer.Infrastructure.Identity.DbContexts;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

public class IdentityService<TUserDto, TRoleDto, TUser, TRole, TKey> : IIdentityService<TUserDto, TRoleDto>
    where TUserDto : UserDto<TKey>
    where TRoleDto : RoleDto<TKey>
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
{
    private readonly IdentityDbContext _identityDbContext;

    private readonly UserManager<TUser> _userManager;
    private readonly RoleManager<TRole> _roleManager;


    private readonly IMapper _mapper;

    public IdentityService(
        IdentityDbContext identityDbContext,
        UserManager<TUser> userManager,
        RoleManager<TRole> roleManager,
        IMapper mapper)
    {
        _identityDbContext = identityDbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    #region User Methods

    public async Task<(Result Result, string? UserId)> CreateUserAsync(TUserDto? userDto)
    {
        var user = _mapper.Map<TUser>(userDto);

        var identityResult = await _userManager.CreateAsync(user);

        return (identityResult.ToApplicationResult(), user.Id.ToString());
    }

    public async Task<(Result Result, TUserDto UserDto)> UpdateUserAsync(TUserDto userDto)
    {
        var user = _mapper.Map<TUser>(userDto);

        var identityResult = await _userManager.UpdateAsync(user);

        return (identityResult.ToApplicationResult(), userDto);
    }

    public async Task<bool> UserExistsAsync(string? userId)
    {
        return await FindUserByIdAsync(userId) is not null;
    }

    public async Task<TUserDto?> FindUserByIdAsync(string? userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var userDto = _mapper.Map<TUserDto>(user);

        return userDto;
    }

    public async Task<string?> GetUserNameAsync(string? userId)
    {
        var user = await FindUserByIdAsync(userId);

        return user?.UserName;
    }

    public async Task<string?> GetDisplayNameAsync(string? userId)
    {
        var user = await FindUserByIdAsync(userId);

        return user?.DisplayName;
    }

    public Task<PaginatedList<TUserDto>> GetUsersAsync(string? search, int pageIndex = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return Task.Run(async () =>
        {
            Expression<Func<TUser, bool>> searchExpression = user => !string.IsNullOrWhiteSpace(user.Email) &&
                                                                               !string.IsNullOrWhiteSpace(search) &&
                                                                               (user.UserName.Contains(search,
                                                                                       StringComparison.OrdinalIgnoreCase)
                                                                                   || user.Email.Contains(search,
                                                                                       StringComparison.OrdinalIgnoreCase));

            await _userManager.Users.Where(searchExpression)
                .ToListAsync(cancellationToken);

            var users = await _userManager.Users.Where(searchExpression).PaginatedListAsync(pageIndex, pageSize);

            var usersDto = _mapper.Map<PaginatedList<TUserDto>>(users);

            return usersDto;
        }, cancellationToken);
    }

    public Task<PaginatedList<TUserDto>> GetRoleUsersAsync(string? roleName, string? search,
        int pageIndex = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return Task.Run(async () =>
        {
            Expression<Func<TUserDto, bool>> searchExpression = x => x.Email != null && x.UserName != null &&
                                                                     !string.IsNullOrWhiteSpace(x.Email) &&
                                                                     !string.IsNullOrWhiteSpace(search) &&
                                                                     (x.UserName.Contains(search,
                                                                          StringComparison.OrdinalIgnoreCase) ||
                                                                      x.Email.Contains(search,
                                                                          StringComparison.OrdinalIgnoreCase));

            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

            var usersInRoleDto = _mapper.Map<IQueryable<TUserDto>>(usersInRole);

            var users = await usersInRoleDto
                .Where(searchExpression)
                .PaginatedListAsync(pageIndex, pageSize);

            return users;
        }, cancellationToken);
    }

    public async Task<PaginatedList<TUserDto>> GetClaimUsersAsync(string? claimType, string? claimValue,
        int pageIndex = 1, int pageSize = 10)
    {
        var users = await _identityDbContext.Users
            .Join(_identityDbContext.UserClaims, u => u.Id,
                uc => uc.UserId, (u, uc) => new {u, uc}).Where(x =>
                x.uc.ClaimType.Equals(claimType) && x.uc.ClaimValue.Equals(claimValue))
            .Select(x => x.u).Distinct().PaginatedListAsync(pageIndex, pageSize);

        var usersDto = _mapper.Map<PaginatedList<TUserDto>>(users);

        return usersDto;
    }

    public async Task<PaginatedList<UserClaimDto>> GetUserClaimsAsync(string? userId, int pageIndex = 1,
        int pageSize = 10)
    {
        var userClaims = await _identityDbContext.UserClaims.Where(x => x.UserId.Equals(userId))
            .PaginatedListAsync(pageIndex, pageSize);

        var userClaimsDto = _mapper.Map<PaginatedList<UserClaimDto>>(userClaims);

        return userClaimsDto;
    }

    public async Task<PaginatedList<RoleClaimDto>> GetUserRoleClaimsAsync(string? userId,
        string? claimSearchText, int pageIndex = 1, int pageSize = 10)
    {
        Expression<Func<ApplicationRoleClaim, bool>> searchExpression = roleClaim =>
            !string.IsNullOrWhiteSpace(claimSearchText) &&
            roleClaim.ClaimType!.Contains(claimSearchText, StringComparison.OrdinalIgnoreCase);

        var roleClaims = await _identityDbContext.UserRoles.Where(x => x.UserId.Equals(userId)).Join(
            _identityDbContext.RoleClaims.Where(searchExpression), ur => ur.RoleId, rc => rc.RoleId,
            (ur, rc) => rc).PaginatedListAsync(pageIndex, pageSize);

        var roleClaimsDto = _mapper.Map<PaginatedList<RoleClaimDto>>(roleClaims);

        return roleClaimsDto;
    }

    public Task<UserClaimDto?> GetUserClaimAsync(string? userId, int claimId)
    {
        var userClaim =
            _identityDbContext.UserClaims.FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.Id == claimId);

        var userClaimDto = _mapper.Map<Task<UserClaimDto?>>(userClaim);

        return userClaimDto;
    }

    public Task<UserClaimDto?> GetUserClaimAsync(string? userId, string? claimType, string? claimValue)
    {
        var userClaim = _identityDbContext.UserClaims.FirstOrDefaultAsync(x =>
            x.UserId.Equals(userId) && x.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
            x.ClaimValue.Equals(claimValue, StringComparison.OrdinalIgnoreCase));

        var userClaimDto = _mapper.Map<Task<UserClaimDto?>>(userClaim);

        return userClaimDto;
    }

    public async Task<Result?> CreateUserClaimAsync(UserClaimDto userClaim)
    {
        var userDto = await FindUserByIdAsync(userClaim.UserId);

        var user = _mapper.Map<TUser>(userDto);

        return (await _userManager.AddClaimAsync(user!, new Claim(userClaim.ClaimType!, userClaim.ClaimValue!)))
            .ToApplicationResult();
    }

    public async Task<Result> UpdateUserClaimAsync(UserClaimDto newUserClaim)
    {
        var userDto  = await FindUserByIdAsync(newUserClaim.UserId);

        var user = _mapper.Map<TUser>(userDto);

        var userClaim = await GetUserClaimAsync(newUserClaim.Id);

        if (!string.IsNullOrWhiteSpace(userClaim?.ClaimType) && !string.IsNullOrWhiteSpace(userClaim.ClaimValue) && user is not null)
        {
            await _userManager.RemoveClaimAsync(user, new Claim(userClaim.ClaimType, userClaim.ClaimValue));
        }

        return (await _userManager.AddClaimAsync(user!, new Claim(newUserClaim.ClaimType!, newUserClaim.ClaimValue!)))
            .ToApplicationResult();
    }

    public async Task<Result> DeleteUserClaimAsync(string? userId, int claimId)
    {
        var userDto = await FindUserByIdAsync(userId);

        var user = _mapper.Map<TUser>(userDto);

        var userClaim = await GetUserClaimAsync(claimId);

        if (userClaim is not null)
            return (await _userManager.RemoveClaimAsync(user!,
                    new Claim(userClaim.ClaimType!, userClaim.ClaimValue!)))
                .ToApplicationResult();

        return Result.Failure(new List<string>{ "User Claim not found." });
    }

    public Task<UserClaimDto?> GetUserClaimAsync(int claimId)
    {
        var userClaim = _identityDbContext.UserClaims.FirstOrDefaultAsync(x => x.Id == claimId);

        var userClaimDto = _mapper.Map<Task<UserClaimDto?>>(userClaim);

        return userClaimDto;
    }

    public async Task<IReadOnlyList<UserLoginInfo>> GetUserProvidersAsync(string? userId)
    {
        var userDto = await FindUserByIdAsync(userId);

        var user = _mapper.Map<TUser>(userDto);

        var userLoginInfos = await _userManager.GetLoginsAsync(user!);

        return userLoginInfos.ToList();
    }

    public async Task<UserLoginDto?> GetUserProviderAsync(string? userId, string? providerKey)
    {
        var userLogin = await _identityDbContext.UserLogins.FirstOrDefaultAsync(x =>
            x.UserId.Equals(userId) && x.ProviderKey.Equals(providerKey));

        var userLoginDto = _mapper.Map<UserLoginDto?>(userLogin);

        return userLoginDto;
    }

    public Task<UserLoginDto?> GetUserProviderAsync(string? userId, string? providerKey, string? loginProvider)
    {
        var userLogin = _identityDbContext.UserLogins.FirstOrDefaultAsync(x => x.UserId.Equals(userId)
                                                                               && x.ProviderKey.Equals(providerKey)
                                                                               && x.LoginProvider.Equals(loginProvider));

        var userLoginDto = _mapper.Map<Task<UserLoginDto?>>(userLogin);

        return userLoginDto;
    }

    public async Task<Result> DeleteUserProviderAsync(string? userId, string? providerKey, string? loginProvider)
    {
        var userDto = await FindUserByIdAsync(userId);

        var user = _mapper.Map<TUser>(userDto);

        var userLogin = await GetUserProviderAsync(userId, providerKey, loginProvider);

        return (await _userManager.RemoveLoginAsync(user!, userLogin?.LoginProvider, userLogin?.ProviderKey)).ToApplicationResult();
    }

    public async Task<Result> ChangeUserPasswordAsync(string? userId, string? password)
    {
        var userDto = await FindUserByIdAsync(userId);

        var user = _mapper.Map<TUser>(userDto);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user!);

        return (await _userManager.ResetPasswordAsync(user!, token, password)).ToApplicationResult();
    }

    public async Task<Result> DeleteUserAsync(string? userId)
    {
        var userDto = await FindUserByIdAsync(userId);

        var user = _mapper.Map<TUser>(userDto);

        return (await _userManager.DeleteAsync(user!)).ToApplicationResult();
    }

    #endregion


    #region Role Methods

    public async Task<TRoleDto?> FindRoleByIdAsync(string? roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);

        var roleDto = _mapper.Map<TRoleDto>(role);

        return roleDto;
    }

    public async Task<bool> RoleExistsAsync(string? roleId)
    {
        return await FindRoleByIdAsync(roleId) is not null;
    }

    public async Task<IReadOnlyList<TRoleDto>> GetRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();

        var rolesDto = _mapper.Map<IReadOnlyList<TRoleDto>>(roles);

        return rolesDto;
    }

    public async Task<PaginatedList<TRoleDto>> GetRolesAsync(string? search, int pageIndex = 1,
        int pageSize = 10)
    {
        Expression<Func<TRole, bool>> searchExpression = role =>
            !string.IsNullOrWhiteSpace(search) &&
            role.Name.Contains(search, StringComparison.OrdinalIgnoreCase);

        var roles = await _roleManager.Roles.Where(searchExpression)
            .PaginatedListAsync(pageIndex, pageSize);

        var rolesDto = _mapper.Map<PaginatedList<TRoleDto>>(roles);

        return rolesDto;
    }

    public async Task<PaginatedList<TRoleDto>> GetUserRolesAsync(string? userId, int pageIndex = 1,
        int pageSize = 10)
    {
        var roles = await _identityDbContext.Roles.Join(_identityDbContext.UserRoles,
            r => r.Id, ur => ur.UserId,
            (r, ur) => new {r, ur}).Where(x =>
            x.ur.UserId.Equals(userId)).Select(x => x.r).PaginatedListAsync(pageIndex, pageSize);

        var rolesDto = _mapper.Map<PaginatedList<TRoleDto>>(roles);

        return rolesDto;
    }

    public async Task<(Result Result, string? RoleId)> CreateRoleAsync(TRoleDto roleDto)
    {
        var role = _mapper.Map<TRole>(roleDto);

        var identityResult = await _roleManager.CreateAsync(role);

        return (identityResult.ToApplicationResult(), role.Id.ToString());
    }

    public async Task<(Result Result, string? RoleId)> UpdateRoleAsync(TRoleDto roleDto)
    {
        var role = _mapper.Map<TRole>(roleDto);

        var identityResult = await _roleManager.UpdateAsync(role);

        return (identityResult.ToApplicationResult(), role.Id.ToString());
    }

    public async Task<(Result Result, string? RoleId)> DeleteRoleAsync(TRoleDto roleDto)
    {
        var role = _mapper.Map<TRole>(roleDto);

        var identityResult = await _roleManager.DeleteAsync(role);

        return (identityResult.ToApplicationResult(), role.Id.ToString());
    }

    public async Task<PaginatedList<RoleClaimDto>> GetRoleClaimsAsync(string? roleId, int pageIndex = 1,
        int pageSize = 10)
    {
        var roleClaims = await _identityDbContext.RoleClaims.Where(x => x.RoleId.Equals(roleId))
            .PaginatedListAsync(pageIndex, pageSize);

        var roleClaimsDto = _mapper.Map<PaginatedList<RoleClaimDto>>(roleClaims);

        return roleClaimsDto;
    }

    public Task<RoleClaimDto?> GetRoleClaimAsync(string? roleId, int claimId)
    {
        var roleClaim =
            _identityDbContext.RoleClaims.FirstOrDefaultAsync(x => x.RoleId.Equals(roleId) && x.Id == claimId);

        var roleClaimDto = _mapper.Map<Task<RoleClaimDto?>>(roleClaim);

        return roleClaimDto;
    }

    public async Task<Result> CreateRoleClaimAsync(RoleClaimDto roleClaim)
    {
        var roleDto = await FindRoleByIdAsync(roleClaim.RoleId);

        var role = _mapper.Map<TRole>(roleDto);

        return (await _roleManager.AddClaimAsync(role!, new Claim(roleClaim.ClaimType!, roleClaim.ClaimValue!)))
            .ToApplicationResult();
    }

    public async Task<Result> UpdateRoleClaimAsync(RoleClaimDto newRoleClaim)
    {
        var roleDto = await FindRoleByIdAsync(newRoleClaim.RoleId);

        var role = _mapper.Map<TRole>(roleDto);

        var roleClaim = await _identityDbContext.RoleClaims.FirstOrDefaultAsync(x => x.Id == newRoleClaim.Id);

        if (!string.IsNullOrWhiteSpace(roleClaim?.ClaimType) && !string.IsNullOrWhiteSpace(roleClaim.ClaimValue))
        {
            await _roleManager.RemoveClaimAsync(role!, new Claim(roleClaim.ClaimType, roleClaim.ClaimValue));
        }

        return (await _roleManager.AddClaimAsync(role!, new Claim(newRoleClaim.ClaimType!, newRoleClaim.ClaimValue!)))
            .ToApplicationResult();
    }

    public async Task<Result> DeleteRoleClaimAsync(string? roleId, int claimId)
    {
        var roleDto = await FindRoleByIdAsync(roleId);

        var role = _mapper.Map<TRole>(roleDto);

        var roleClaim = await _identityDbContext.RoleClaims.FirstOrDefaultAsync(x => x.Id == claimId);

        if (roleClaim is not null)
        {
            return (await _roleManager.RemoveClaimAsync(role!,
                    new Claim(roleClaim.ClaimType, roleClaim.ClaimValue)))
                .ToApplicationResult();
        }

        return Result.Failure(new List<string>{ "Role Claim not found" });
    }

    #endregion
}