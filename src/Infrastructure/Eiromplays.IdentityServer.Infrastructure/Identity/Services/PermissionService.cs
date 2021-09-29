using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Eiromplays.IdentityServer.Application.Common.Interface;
using Eiromplays.IdentityServer.Application.Common.Mappings;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Domain.Entities;
using Eiromplays.IdentityServer.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services
{
    public class PermissionService
    {
        private readonly PermissionDbContext _permissionDbContext;
        private readonly IIdentityService _identityService;

        public PermissionService(PermissionDbContext permissionDbContext, IIdentityService identityService)
        {
            _permissionDbContext = permissionDbContext;
            _identityService = identityService;
        }

        public async Task<bool> HasPermissionAsync(string? userId)
        {
            return true;
        }

        public Task<PaginatedList<Permission>> GetPermissionsAsync(string? search, int pageIndex = 1, int pageSize = 10)
        {
            return Task.Run(async () =>
            {
                Expression<Func<Permission, bool>> searchExpression = x =>
                    !string.IsNullOrWhiteSpace(x.Id) && !string.IsNullOrWhiteSpace(x.Name) &&
                    !string.IsNullOrWhiteSpace(search) && x.Id.Contains(search);

                var permissions = await _permissionDbContext.Permissions!.Where(searchExpression)
                    .PaginatedListAsync(pageIndex, pageSize);

                return permissions;
            });
        }

        public Task<List<Permission>> GetPermissionsAsync()
        {
            return _permissionDbContext.Permissions!.ToListAsync();
        }

        public async Task<Permission?> GetPermissionAsync(string? search)
        {
            var permission = await _permissionDbContext.Permissions!.FirstOrDefaultAsync(x =>
                !string.IsNullOrWhiteSpace(x.Id) && !string.IsNullOrWhiteSpace(x.Name) && (x.Id.Equals(search) ||
                    x.Name.Equals(search, StringComparison.OrdinalIgnoreCase)));

            return permission;
        }

        public async Task<Permission?> FindPermissionById(string? id)
        {
            var permission =
               await _permissionDbContext.Permissions!.FirstOrDefaultAsync(x =>
                    !string.IsNullOrWhiteSpace(x.Id) && x.Id.Equals(id));

            return permission;
        }
    }
}