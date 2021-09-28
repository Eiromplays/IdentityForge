using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Eiromplays.IdentityServer.Application.Common.Interface;
using Eiromplays.IdentityServer.Application.Common.Mappings;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Infrastructure.Extensions;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.Infrastructure.Persistence.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IdentityDbContext _identityDbContext;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IUserClaimsPrincipalFactory<ApplicationUser?> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;

        public IdentityService(
            IdentityDbContext identityDbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IUserClaimsPrincipalFactory<ApplicationUser?> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService)
        {
            _identityDbContext = identityDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
        }

        #region User Methods

        public async Task<(Result Result, string UserId)> CreateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.CreateAsync(user);

            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<(IdentityResult identityResult, ApplicationUser user)> UpdateUserAsync(ApplicationUser user)
        {
            var identityResult = await _userManager.UpdateAsync(user);

            return (identityResult, user);
        }

        public async Task<bool> UserExistsAsync(string? userId)
        {
            return await GetUserAsync(userId) is not null;
        }

        public async Task<ApplicationUser?> GetUserAsync(string? userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<string?> GetUserNameAsync(string? userId)
        {
            var user = await GetUserAsync(userId);

            return user?.UserName;
        }

        public async Task<string?> GetDisplayNameAsync(string? userId)
        {
            var user = await GetUserAsync(userId);

            return user?.DisplayName;
        }

        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<PaginatedList<ApplicationUser>> GetUsersAsync(string? search, int pageIndex = 1, int pageSize = 10)
        {
            Expression<Func<ApplicationUser, bool>> searchExpression = x => x.Email != null && search != null &&
                                                                            (x.UserName.Contains(search,
                                                                                 StringComparison.OrdinalIgnoreCase)
                                                                             || x.Email.Contains(search,
                                                                                 StringComparison.OrdinalIgnoreCase));

            var users = await _userManager.Users.Where(searchExpression).PaginatedListAsync(pageIndex, pageSize);

            return users;
        }

        public async Task<PaginatedList<ApplicationUser>> GetRoleUsersAsync(string? roleName, string? search,
            int pageIndex = 1, int pageSize = 10)
        {
            var users = await (await _userManager.GetUsersInRoleAsync(roleName)).Where(x =>
                !string.IsNullOrWhiteSpace(x.Email) && !string.IsNullOrWhiteSpace(search) &&
                (x.UserName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                 x.Email.Contains(search, StringComparison.OrdinalIgnoreCase))).AsQueryable().PaginatedListAsync(pageIndex, pageSize);

            return users;
        }

        public async Task<PaginatedList<ApplicationUser>> GetClaimUsersAsync(string? claimType, string? claimValue,
            int pageIndex = 1, int pageSize = 10)
        {
            var users = await _identityDbContext.Users
                .Join(_identityDbContext.UserClaims, u => u.Id, 
                    uc => uc.UserId, (u, uc) => new {u, uc}).Where(x =>
                    x.uc.ClaimType.Equals(claimType) && x.uc.ClaimValue.Equals(claimValue))
                .Select(x => x.u).Distinct().PaginatedListAsync(pageIndex, pageSize);

            return users;
        }

        public async Task<PaginatedList<ApplicationUserClaim>> GetUserClaimsAsync(string? userId, int pageIndex = 1,
            int pageSize = 10)
        {
            var claims = await _identityDbContext.UserClaims.Where(x => x.UserId.Equals(userId))
                .PaginatedListAsync(pageIndex, pageSize);

            return claims;
        }

        public async Task<PaginatedList<ApplicationRoleClaim>> GetUserRoleClaimsAsync(string? userId,
            string? claimSearchText, int pageIndex = 1, int pageSize = 10)
        {
            Expression<Func<ApplicationRoleClaim, bool>> searchExpression = x =>
                !string.IsNullOrWhiteSpace(claimSearchText) &&
                x.ClaimType.Contains(claimSearchText, StringComparison.OrdinalIgnoreCase);

            var claims = await _identityDbContext.UserRoles.Where(x => x.UserId.Equals(userId)).Join(
                _identityDbContext.RoleClaims.Where(searchExpression), ur => ur.RoleId, rc => rc.RoleId,
                (ur, rc) => rc).PaginatedListAsync(pageIndex, pageSize);

            return claims;
        }

        public Task<ApplicationUserClaim?> GetUserClaimAsync(string? userId, int claimId)
        {
            return _identityDbContext.UserClaims.FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.Id == claimId);
        }

        public Task<ApplicationUserClaim?> GetUserClaimAsync(string? userId, string? claimType, string? claimValue)
        {
            return _identityDbContext.UserClaims.FirstOrDefaultAsync(x =>
                x.UserId.Equals(userId) && x.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
                x.ClaimValue.Equals(claimValue, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Result?> CreateUserClaimAsync(ApplicationUserClaim userClaim)
        {
            var user = await GetUserAsync(userClaim.UserId);

            return (await _userManager.AddClaimAsync(user!, new Claim(userClaim.ClaimType, userClaim.ClaimValue)))
                .ToApplicationResult();
        }

        #endregion


        #region Role Methods

        public async Task<PaginatedList<ApplicationRole>> GetUserRolesAsync(string? userId, int pageIndex = 1,
            int pageSize = 10)
        {
            var roles = await _identityDbContext.Roles.Join(_identityDbContext.UserRoles, 
                r => r.Id, ur => ur.UserId,
                (r, ur) => new {r, ur}).Where(x => 
                            x.ur.UserId.Equals(userId)).Select(x => x.r).PaginatedListAsync(pageIndex, pageSize);

            return roles;
        }

        #endregion
    }
}
