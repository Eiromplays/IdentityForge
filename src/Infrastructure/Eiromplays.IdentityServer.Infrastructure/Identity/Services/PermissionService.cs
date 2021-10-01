using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
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

        public bool AutoSaveChanges { get; set; } = true;


        public PermissionService(PermissionDbContext permissionDbContext, IIdentityService identityService)
        {
            _permissionDbContext = permissionDbContext;
            _identityService = identityService;
        }

        protected Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return AutoSaveChanges ? _permissionDbContext.SaveChangesAsync(cancellationToken) : Task.CompletedTask;
        }

        public async Task<bool> HasPermissionAsync(string? userId, CancellationToken cancellationToken = default)
        {
            return true;
        }

        public Task<PaginatedList<Permission>> GetPermissionsAsync(string? search, int pageIndex = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return Task.Run(async () =>
            {
                Expression<Func<Permission, bool>> searchExpression = x =>
                    !string.IsNullOrWhiteSpace(x.Id) && !string.IsNullOrWhiteSpace(x.Name) &&
                    !string.IsNullOrWhiteSpace(search) && x.Id.Contains(search);

                var permissions = await _permissionDbContext.Permissions!.Where(searchExpression)
                    .PaginatedListAsync(pageIndex, pageSize);

                return permissions;
            }, cancellationToken);
        }

        public Task<List<Permission>> GetPermissionsAsync()
        {
            return _permissionDbContext.Permissions!.ToListAsync();
        }

        public async Task<Permission?> GetPermissionAsync(string? search, CancellationToken cancellationToken = default)
        {
            var permission = await _permissionDbContext.Permissions!.FirstOrDefaultAsync(x =>
                !string.IsNullOrWhiteSpace(x.Id) && !string.IsNullOrWhiteSpace(x.Name) && (x.Id.Equals(search) ||
                    x.Name.Equals(search, StringComparison.OrdinalIgnoreCase)), cancellationToken: cancellationToken);

            return permission;
        }

        public async Task<Permission?> GetPermissionByIdAsync(string? id, CancellationToken cancellationToken = default)
        {
            var permission =
               await _permissionDbContext.Permissions!.FirstOrDefaultAsync(x =>
                    !string.IsNullOrWhiteSpace(x.Id) && x.Id.Equals(id), cancellationToken: cancellationToken);

            return permission;
        }

        public async Task<string?> DeletePermissionAsync(string? permissionId, CancellationToken cancellationToken = default)
        {
            var permission = await GetPermissionByIdAsync(permissionId, cancellationToken);

            if (permission != null)
            {
                _permissionDbContext.Permissions?.Remove(permission);
            }

            await SaveChangesAsync(cancellationToken);

            return permission?.Id;
        }

        public async Task<(Result Result, string?)> CreatePermissionAsync(string permissionName, CancellationToken cancellationToken = default)
        {
            var permissionExists = await GetPermissionAsync(permissionName, cancellationToken) != null;

            if (permissionExists) return (Result.Failure(new List<string> { $"Permission with name {permissionName} already exists." }),  null);

            var permission = new Permission(permissionName);

            if (_permissionDbContext.Permissions != null)
            {
                await _permissionDbContext.Permissions.AddAsync(permission, cancellationToken);
            }

            await SaveChangesAsync(cancellationToken);

            return (Result.Success(), permission.Name);
        }
    }
}