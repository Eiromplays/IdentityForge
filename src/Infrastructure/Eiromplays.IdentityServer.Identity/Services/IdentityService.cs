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
using AutoMapper.QueryableExtensions;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

public class IdentityService<TKey, TUserDto, TRoleDto, TUserClaimDto, TRoleClaimDto, TUserLoginDto, TUser, TRole> : IIdentityService<TUserDto, TRoleDto>
    where TKey : IEquatable<TKey>
    where TUserDto : UserDto<TKey>
    where TRoleDto : RoleDto<TKey>
    where TUserClaimDto : UserClaimDto<TKey>
    where TRoleClaimDto : RoleClaimDto<TKey>
    where TUserLoginDto : UserLoginDto<TKey>
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
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

            var users = await _userManager.Users.Where(searchExpression)
                .ProjectTo<TUserDto>(_mapper.ConfigurationProvider).PaginatedListAsync(pageIndex, pageSize);

            return users;
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

            var usersInRole = (await _userManager.GetUsersInRoleAsync(roleName)).AsQueryable()
                .ProjectTo<TUserDto>(_mapper.ConfigurationProvider);

            var users = await usersInRole
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

    public async Task<PaginatedList<TUserClaimDto>> GetUserClaimsAsync(string? userId, int pageIndex = 1,
        int pageSize = 10)
    {
        var userClaims = await _identityDbContext.UserClaims.Where(x => x.UserId.Equals(userId))
            .ProjectTo<TUserClaimDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(pageIndex, pageSize);

        return userClaims;
    }

    public async Task<PaginatedList<TRoleClaimDto>> GetUserRoleClaimsAsync(string? userId,
        string? claimSearchText, int pageIndex = 1, int pageSize = 10)
    {
        Expression<Func<ApplicationRoleClaim, bool>> searchExpression = roleClaim =>
            !string.IsNullOrWhiteSpace(claimSearchText) &&
            roleClaim.ClaimType!.Contains(claimSearchText, StringComparison.OrdinalIgnoreCase);

        var roleClaims = await _identityDbContext.UserRoles.Where(x => x.UserId.Equals(userId)).Join(
            _identityDbContext.RoleClaims.Where(searchExpression), ur => ur.RoleId, rc => rc.RoleId,
            (ur, rc) => rc).ProjectTo<TRoleClaimDto>(_mapper.ConfigurationProvider).PaginatedListAsync(pageIndex, pageSize);

        return roleClaims;
    }

    public async Task<TUserClaimDto?> GetUserClaimAsync(string? userId, int claimId)
    {
        var userClaim = await _identityDbContext.UserClaims.FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.Id == claimId);

        var userClaimDto = _mapper.Map<TUserClaimDto?>(userClaim);

        return userClaimDto;
    }

    public async Task<TUserClaimDto?> GetUserClaimAsync(string? userId, string? claimType, string? claimValue)
    {
        var userClaim = await _identityDbContext.UserClaims.FirstOrDefaultAsync(x =>
            x.UserId.Equals(userId) && x.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
            x.ClaimValue.Equals(claimValue, StringComparison.OrdinalIgnoreCase));

        var userClaimDto = _mapper.Map<TUserClaimDto?>(userClaim);

        return userClaimDto;
    }

    public async Task<Result?> CreateUserClaimAsync(TUserClaimDto userClaimDto)
    {
        var userDto = await FindUserByIdAsync(userClaimDto.UserId?.ToString());

        var user = _mapper.Map<TUser>(userDto);

        return (await _userManager.AddClaimAsync(user!, new Claim(userClaimDto.ClaimType!, userClaimDto.ClaimValue!)))
            .ToApplicationResult();
    }

    public async Task<Result> UpdateUserClaimAsync(TUserClaimDto newUserClaimDto)
    {
        var userDto  = await FindUserByIdAsync(newUserClaimDto.UserId?.ToString());

        var user = _mapper.Map<TUser>(userDto);

        var userClaimDto = await GetUserClaimAsync(newUserClaimDto.ClaimId);

        if (!string.IsNullOrWhiteSpace(userClaimDto?.ClaimType) && !string.IsNullOrWhiteSpace(userClaimDto?.ClaimValue) && user is not null)
        {
            await _userManager.RemoveClaimAsync(user, new Claim(userClaimDto?.ClaimType!, userClaimDto?.ClaimValue!));
        }

        return (await _userManager.AddClaimAsync(user!, new Claim(newUserClaimDto.ClaimType!, newUserClaimDto.ClaimValue!)))
            .ToApplicationResult();
    }

    public async Task<Result> DeleteUserClaimAsync(string? userId, int claimId)
    {
        var userDto = await FindUserByIdAsync(userId);

        var user = _mapper.Map<TUser>(userDto);

        var userClaimDto = await GetUserClaimAsync(claimId);

        if (userClaimDto is not null)
            return (await _userManager.RemoveClaimAsync(user!,
                    new Claim(userClaimDto?.ClaimType!, userClaimDto?.ClaimValue!)))
                .ToApplicationResult();

        return Result.Failure(new List<string>{ "User Claim not found." });
    }

    public async Task<TUserClaimDto?> GetUserClaimAsync(int claimId)
    {
        var userClaim = await _identityDbContext.UserClaims.FirstOrDefaultAsync(x => x.Id == claimId);

        var userClaimDto = _mapper.Map<TUserClaimDto?>(userClaim);

        return userClaimDto;
    }

    public async Task<IReadOnlyList<UserLoginInfo>> GetUserProvidersAsync(string? userId)
    {
        var userDto = await FindUserByIdAsync(userId);

        var user = _mapper.Map<TUser>(userDto);

        var userLoginInfos = await _userManager.GetLoginsAsync(user!);

        return userLoginInfos.ToList();
    }

    public async Task<TUserLoginDto?> GetUserProviderAsync(string? userId, string? providerKey)
    {
        var userLogin = await _identityDbContext.UserLogins.FirstOrDefaultAsync(x =>
            x.UserId.Equals(userId) && x.ProviderKey.Equals(providerKey));

        var userLoginDto = _mapper.Map<TUserLoginDto?>(userLogin);

        return userLoginDto;
    }

    public async Task<TUserLoginDto?> GetUserProviderAsync(string? userId, string? providerKey, string? loginProvider)
    {
        var userLogin = await _identityDbContext.UserLogins.FirstOrDefaultAsync(x => x.UserId.Equals(userId)
                                                                               && x.ProviderKey.Equals(providerKey)
                                                                               && x.LoginProvider.Equals(loginProvider));

        var userLoginDto = _mapper.Map<TUserLoginDto?>(userLogin);

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

    public async Task<PaginatedList<TRoleClaimDto>> GetRoleClaimsAsync(string? roleId, int pageIndex = 1,
        int pageSize = 10)
    {
        var roleClaims = await _identityDbContext.RoleClaims.Where(x => x.RoleId.Equals(roleId)).ProjectTo<TRoleClaimDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(pageIndex, pageSize);

        return roleClaims;
    }

    public async Task<TRoleClaimDto?> GetRoleClaimAsync(string? roleId, int claimId)
    {
        var roleClaim = await
            _identityDbContext.RoleClaims.FirstOrDefaultAsync(x => x.RoleId.Equals(roleId) && x.Id == claimId);

        var roleClaimDto = _mapper.Map<TRoleClaimDto?>(roleClaim);

        return roleClaimDto;
    }

    public async Task<Result> CreateRoleClaimAsync(TRoleClaimDto roleClaim)
    {
        var roleDto = await FindRoleByIdAsync(roleClaim.RoleId?.ToString());

        var role = _mapper.Map<TRole>(roleDto);

        return (await _roleManager.AddClaimAsync(role!, new Claim(roleClaim.ClaimType!, roleClaim.ClaimValue!)))
            .ToApplicationResult();
    }

    public async Task<Result> UpdateRoleClaimAsync(TRoleClaimDto newRoleClaim)
    {
        var roleDto = await FindRoleByIdAsync(newRoleClaim.RoleId?.ToString());

        var role = _mapper.Map<TRole>(roleDto);

        var roleClaimDto = await GetRoleClaimAsync(role.Id.ToString(), newRoleClaim.ClaimId);

        if (!string.IsNullOrWhiteSpace(roleClaimDto?.ClaimType) && !string.IsNullOrWhiteSpace(roleClaimDto.ClaimValue))
        {
            await _roleManager.RemoveClaimAsync(role!, new Claim(roleClaimDto.ClaimType, roleClaimDto.ClaimValue));
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