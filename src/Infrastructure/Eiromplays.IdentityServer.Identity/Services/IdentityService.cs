using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eiromplays.IdentityServer.Application.Common.Mappings;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.DTOs.Role;
using Eiromplays.IdentityServer.Application.Identity.DTOs.User;
using Eiromplays.IdentityServer.Infrastructure.Extensions;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;
using Eiromplays.IdentityServer.Application.Identity.Common.Interfaces;
using Eiromplays.IdentityServer.Infrastructure.Identity.Persistence.DbContexts;
using Microsoft.AspNetCore.Authorization;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly IdentityDbContext _identityDbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMapper _mapper;

    public IdentityService(
        IdentityDbContext identityDbContext,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IMapper mapper,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService)
    {
        _identityDbContext = identityDbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
    }

    #region User Methods

    public async Task<(Result Result, string? UserId)> CreateUserAsync(UserDto? userDto)
    {
        var user = _mapper.Map<ApplicationUser>(userDto);

        var identityResult = await _userManager.CreateAsync(user);

        return (identityResult.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var userDto = await FindUserByIdAsync(userId);

        var user = _mapper.Map<ApplicationUser>(userDto);

        return user is not null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var userDto = await FindUserByIdAsync(userId);

        var user = _mapper.Map<ApplicationUser>(userDto);

        if (user is null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<(Result Result, string? UserId)> UpdateUserAsync(UserDto userDto)
    {
        var user = _mapper.Map<ApplicationUser>(userDto);

        var identityResult = await _userManager.UpdateAsync(user);

        return (identityResult.ToApplicationResult(), user.Id);
    }

    public async Task<bool> UserExistsAsync(string? userId)
    {
        return await FindUserByIdAsync(userId) is not null;
    }

    public async Task<UserDto?> FindUserByIdAsync(string? userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var userDto = _mapper.Map<UserDto>(user);

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

    public Task<PaginatedList<UserDto>> GetUsersAsync(string? search, int pageIndex = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return Task.Run(async () =>
        {
            Expression<Func<ApplicationUser, bool>> searchExpression = user => !string.IsNullOrWhiteSpace(user.Email) &&
                                                                               !string.IsNullOrWhiteSpace(search) &&
                                                                               (user.UserName.Contains(search,
                                                                                       StringComparison.OrdinalIgnoreCase)
                                                                                   || user.Email.Contains(search,
                                                                                       StringComparison.OrdinalIgnoreCase));

            var users = await _userManager.Users.Where(searchExpression)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider).PaginatedListAsync(pageIndex, pageSize);

            return users;
        }, cancellationToken);
    }

    public Task<PaginatedList<UserDto>> GetRoleUsersAsync(string? roleName, string? search,
        int pageIndex = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return Task.Run(async () =>
        {
            Expression<Func<UserDto, bool>> searchExpression = x => x.Email != null && x.UserName != null &&
                                                                     !string.IsNullOrWhiteSpace(x.Email) &&
                                                                     !string.IsNullOrWhiteSpace(search) &&
                                                                     (x.UserName.Contains(search,
                                                                          StringComparison.OrdinalIgnoreCase) ||
                                                                      x.Email.Contains(search,
                                                                          StringComparison.OrdinalIgnoreCase));

            var usersInRole = (await _userManager.GetUsersInRoleAsync(roleName)).AsQueryable()
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider);

            var users = await usersInRole
                .Where(searchExpression)
                .PaginatedListAsync(pageIndex, pageSize);

            return users;
        }, cancellationToken);
    }

    public async Task<PaginatedList<UserDto>> GetClaimUsersAsync(string? claimType, string? claimValue,
        int pageIndex = 1, int pageSize = 10)
    {
        var users = await _identityDbContext.Users
            .Join(_identityDbContext.UserClaims, u => u.Id,
                uc => uc.UserId, (u, uc) => new {u, uc}).Where(x =>
                x.uc.ClaimType.Equals(claimType) && x.uc.ClaimValue.Equals(claimValue))
            .Select(x => x.u).Distinct().PaginatedListAsync(pageIndex, pageSize);

        var usersDto = _mapper.Map<PaginatedList<UserDto>>(users);

        return usersDto;
    }

    public async Task<PaginatedList<UserClaimDto>> GetUserClaimsAsync(string? userId, int pageIndex = 1,
        int pageSize = 10)
    {
        var userClaims = await _identityDbContext.UserClaims.Where(x => x.UserId.Equals(userId))
            .ProjectTo<UserClaimDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(pageIndex, pageSize);

        return userClaims;
    }

    public async Task<PaginatedList<RoleClaimDto>> GetUserRoleClaimsAsync(string? userId,
        string? claimSearchText, int pageIndex = 1, int pageSize = 10)
    {
        Expression<Func<ApplicationRoleClaim, bool>> searchExpression = roleClaim =>
            !string.IsNullOrWhiteSpace(claimSearchText) &&
            roleClaim.ClaimType!.Contains(claimSearchText, StringComparison.OrdinalIgnoreCase);

        var roleClaims = await _identityDbContext.UserRoles.Where(x => x.UserId.Equals(userId)).Join(
            _identityDbContext.RoleClaims.Where(searchExpression), ur => ur.RoleId, rc => rc.RoleId,
            (ur, rc) => rc).ProjectTo<RoleClaimDto>(_mapper.ConfigurationProvider).PaginatedListAsync(pageIndex, pageSize);

        return roleClaims;
    }

    public async Task<UserClaimDto?> GetUserClaimAsync(string? userId, int claimId)
    {
        var userClaim = await _identityDbContext.UserClaims.FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.Id == claimId);

        var userClaimDto = _mapper.Map<UserClaimDto?>(userClaim);

        return userClaimDto;
    }

    public async Task<UserClaimDto?> GetUserClaimAsync(string? userId, string? claimType, string? claimValue)
    {
        var userClaim = await _identityDbContext.UserClaims.FirstOrDefaultAsync(x =>
            x.UserId.Equals(userId) && x.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
            x.ClaimValue.Equals(claimValue, StringComparison.OrdinalIgnoreCase));

        var userClaimDto = _mapper.Map<UserClaimDto?>(userClaim);

        return userClaimDto;
    }

    public async Task<Result?> CreateUserClaimAsync(UserClaimDto userClaimDto)
    {
        var userDto = await FindUserByIdAsync(userClaimDto.UserId);

        var user = _mapper.Map<ApplicationUser>(userDto);

        return (await _userManager.AddClaimAsync(user!, new Claim(userClaimDto.ClaimType!, userClaimDto.ClaimValue!)))
            .ToApplicationResult();
    }

    public async Task<Result> UpdateUserClaimAsync(UserClaimDto newUserClaimDto)
    {
        var userDto  = await FindUserByIdAsync(newUserClaimDto.UserId);

        var user = _mapper.Map<ApplicationUser>(userDto);

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

        var user = _mapper.Map<ApplicationUser>(userDto);

        var userClaimDto = await GetUserClaimAsync(claimId);

        if (userClaimDto is not null)
            return (await _userManager.RemoveClaimAsync(user!,
                    new Claim(userClaimDto?.ClaimType!, userClaimDto?.ClaimValue!)))
                .ToApplicationResult();

        return Result.Failure(new List<string>{ "User Claim not found." });
    }

    public async Task<UserClaimDto?> GetUserClaimAsync(int claimId)
    {
        var userClaim = await _identityDbContext.UserClaims.FirstOrDefaultAsync(x => x.Id == claimId);

        var userClaimDto = _mapper.Map<UserClaimDto?>(userClaim);

        return userClaimDto;
    }

    public async Task<IReadOnlyList<UserLoginDto>> GetUserProvidersAsync(string? userId)
    {
        var userDto = await FindUserByIdAsync(userId);

        var user = _mapper.Map<ApplicationUser>(userDto);

        var userLoginInfos = await (await _userManager.GetLoginsAsync(user!)).AsQueryable().ProjectTo<UserLoginDto>(_mapper.ConfigurationProvider).ToListAsync();

        return userLoginInfos;
    }

    public async Task<UserLoginDto?> GetUserProviderAsync(string? userId, string? providerKey)
    {
        var userLogin = await _identityDbContext.UserLogins.FirstOrDefaultAsync(x =>
            x.UserId.Equals(userId) && x.ProviderKey.Equals(providerKey));

        var userLoginDto = _mapper.Map<UserLoginDto?>(userLogin);

        return userLoginDto;
    }

    public async Task<UserLoginDto?> GetUserProviderAsync(string? userId, string? providerKey, string? loginProvider)
    {
        var userLogin = await _identityDbContext.UserLogins.FirstOrDefaultAsync(x => x.UserId.Equals(userId)
                                                                               && x.ProviderKey.Equals(providerKey)
                                                                               && x.LoginProvider.Equals(loginProvider));

        var userLoginDto = _mapper.Map<UserLoginDto?>(userLogin);

        return userLoginDto;
    }

    public async Task<Result> DeleteUserProviderAsync(string? userId, string? providerKey, string? loginProvider)
    {
        var userDto = await FindUserByIdAsync(userId);

        var user = _mapper.Map<ApplicationUser>(userDto);

        var userLogin = await GetUserProviderAsync(userId, providerKey, loginProvider);

        return (await _userManager.RemoveLoginAsync(user!, userLogin?.LoginProvider, userLogin?.ProviderKey)).ToApplicationResult();
    }

    public async Task<Result> ChangeUserPasswordAsync(string? userId, string? password)
    {
        var userDto = await FindUserByIdAsync(userId);

        var user = _mapper.Map<ApplicationUser>(userDto);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user!);

        return (await _userManager.ResetPasswordAsync(user!, token, password)).ToApplicationResult();
    }

    public async Task<Result> DeleteUserAsync(string? userId)
    {
        var userDto = await FindUserByIdAsync(userId);

        var user = _mapper.Map<ApplicationUser>(userDto);

        return (await _userManager.DeleteAsync(user!)).ToApplicationResult();
    }

    #endregion


    #region Role Methods

    public async Task<RoleDto?> FindRoleByIdAsync(string? roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);

        var roleDto = _mapper.Map<RoleDto>(role);

        return roleDto;
    }

    public async Task<bool> RoleExistsAsync(string? roleId)
    {
        return await FindRoleByIdAsync(roleId) is not null;
    }

    public async Task<IReadOnlyList<RoleDto>> GetRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();

        var rolesDto = _mapper.Map<IReadOnlyList<RoleDto>>(roles);

        return rolesDto;
    }

    public async Task<PaginatedList<RoleDto>> GetRolesAsync(string? search, int pageIndex = 1,
        int pageSize = 10)
    {
        Expression<Func<ApplicationRole, bool>> searchExpression = role =>
            !string.IsNullOrWhiteSpace(search) &&
            role.Name.Contains(search, StringComparison.OrdinalIgnoreCase);

        var roles = await _roleManager.Roles.Where(searchExpression)
            .PaginatedListAsync(pageIndex, pageSize);

        var rolesDto = _mapper.Map<PaginatedList<RoleDto>>(roles);

        return rolesDto;
    }

    public async Task<PaginatedList<RoleDto>> GetUserRolesAsync(string? userId, int pageIndex = 1,
        int pageSize = 10)
    {
        var roles = await _identityDbContext.Roles.Join(_identityDbContext.UserRoles,
            r => r.Id, ur => ur.UserId,
            (r, ur) => new {r, ur}).Where(x =>
            x.ur.UserId.Equals(userId)).Select(x => x.r).PaginatedListAsync(pageIndex, pageSize);

        var rolesDto = _mapper.Map<PaginatedList<RoleDto>>(roles);

        return rolesDto;
    }

    public async Task<(Result Result, string? RoleId)> CreateRoleAsync(RoleDto roleDto)
    {
        var role = _mapper.Map<ApplicationRole>(roleDto);

        var identityResult = await _roleManager.CreateAsync(role);

        return (identityResult.ToApplicationResult(), role.Id);
    }

    public async Task<(Result Result, string? RoleId)> UpdateRoleAsync(RoleDto roleDto)
    {
        var role = _mapper.Map<ApplicationRole>(roleDto);

        var identityResult = await _roleManager.UpdateAsync(role);

        return (identityResult.ToApplicationResult(), role.Id);
    }

    public async Task<(Result Result, string? RoleId)> DeleteRoleAsync(RoleDto roleDto)
    {
        var role = _mapper.Map<ApplicationRole>(roleDto);

        var identityResult = await _roleManager.DeleteAsync(role);

        return (identityResult.ToApplicationResult(), role.Id);
    }

    public async Task<PaginatedList<RoleClaimDto>> GetRoleClaimsAsync(string? roleId, int pageIndex = 1,
        int pageSize = 10)
    {
        var roleClaims = await _identityDbContext.RoleClaims.Where(x => x.RoleId.Equals(roleId)).ProjectTo<RoleClaimDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(pageIndex, pageSize);

        return roleClaims;
    }

    public async Task<RoleClaimDto?> GetRoleClaimAsync(string? roleId, int claimId)
    {
        var roleClaim = await
            _identityDbContext.RoleClaims.FirstOrDefaultAsync(x => x.RoleId.Equals(roleId) && x.Id == claimId);

        var roleClaimDto = _mapper.Map<RoleClaimDto?>(roleClaim);

        return roleClaimDto;
    }

    public async Task<Result> CreateRoleClaimAsync(RoleClaimDto roleClaim)
    {
        var roleDto = await FindRoleByIdAsync(roleClaim.RoleId);

        var role = _mapper.Map<ApplicationRole>(roleDto);

        return (await _roleManager.AddClaimAsync(role!, new Claim(roleClaim.ClaimType!, roleClaim.ClaimValue!)))
            .ToApplicationResult();
    }

    public async Task<Result> UpdateRoleClaimAsync(RoleClaimDto newRoleClaim)
    {
        var roleDto = await FindRoleByIdAsync(newRoleClaim.RoleId);

        var role = _mapper.Map<ApplicationRole>(roleDto);

        var roleClaimDto = await GetRoleClaimAsync(role.Id, newRoleClaim.ClaimId);

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

        var role = _mapper.Map<ApplicationRole>(roleDto);

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