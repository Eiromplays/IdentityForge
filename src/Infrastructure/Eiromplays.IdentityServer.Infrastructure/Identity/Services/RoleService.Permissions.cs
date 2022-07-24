using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Roles;
using Eiromplays.IdentityServer.Domain.Identity;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Authorization;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class RoleService
{
    public async Task<RoleDto> GetByIdWithPermissionsAsync(string roleId, CancellationToken cancellationToken)
    {
        var role = await GetByIdAsync(roleId);

        role.Permissions = await _db.RoleClaims
            .Where(c => c.RoleId == roleId && c.ClaimType == EiaClaims.Permission)
            .Select(c => c.ClaimValue)
            .ToListAsync(cancellationToken);

        return role;
    }

    public async Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.RoleId);

        _ = role ?? throw new NotFoundException(_t["Role Not Found"]);

        if (role.Name.Equals(EIARoles.Administrator, StringComparison.OrdinalIgnoreCase))
        {
            throw new ConflictException(_t["Not allowed to modify Permissions for this Role."]);
        }

        var currentClaims = await _roleManager.GetClaimsAsync(role);

        // Remove permissions that were previously selected
        foreach (var claim in currentClaims.Where(c => request.Permissions.All(p => p != c.Value)))
        {
            var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
            if (!removeResult.Succeeded)
            {
                throw new InternalServerException(_t["Update permissions failed."], removeResult.GetErrors(_t));
            }
        }

        // Add all permissions that were not previously selected
        foreach (string permission in request.Permissions.Where(c => currentClaims.All(p => p.Value != c)))
        {
            if (string.IsNullOrEmpty(permission)) continue;

            _db.RoleClaims.Add(new ApplicationRoleClaim
            {
                RoleId = role.Id,
                ClaimType = EiaClaims.Permission,
                ClaimValue = permission,
                CreatedBy = _currentUser.GetUserId(),
                LastModifiedBy = _currentUser.GetUserId()
            });
            await _db.SaveChangesAsync(cancellationToken);
        }

        await _events.PublishAsync(new ApplicationRoleUpdatedEvent(role.Id, role.Name, true));

        return _t["Permissions Updated."];
    }
}