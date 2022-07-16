using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Authorization;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class UserService
{
    public async Task<List<UserRoleDto>> GetRolesAsync(string userId, CancellationToken cancellationToken)
    {
        var userRoles = new List<UserRoleDto>();

        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);
        var roles = await _roleManager.Roles.AsNoTracking().ToListAsync(cancellationToken);

        foreach (var role in roles)
        {
            userRoles.Add(new UserRoleDto
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Description = role.Description,
                Enabled = await _userManager.IsInRoleAsync(user, role.Name)
            });
        }

        return userRoles;
    }

    public async Task<string> AssignRolesAsync(string userId, UserRolesRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        // Check if the user is an admin for which the admin role is getting disabled
        if (await _userManager.IsInRoleAsync(user, EIARoles.Administrator)
            && request.UserRoles.Any(a => !a.Enabled && a.RoleName == EIARoles.Administrator))
        {
            // Get count of users in Admin Role
            int adminCount = (await _userManager.GetUsersInRoleAsync(EIARoles.Administrator)).Count;

            if (adminCount <= 2)
            {
                throw new ConflictException(_t["Tenant should have at least 2 Admins."]);
            }
        }

        foreach (var userRole in request.UserRoles)
        {
            // Check if Role Exists
            if (await _roleManager.FindByNameAsync(userRole.RoleName) is null) continue;

            if (userRole.Enabled)
            {
                if (!await _userManager.IsInRoleAsync(user, userRole.RoleName))
                {
                    await _userManager.AddToRoleAsync(user, userRole.RoleName);
                }
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user, userRole.RoleName);
            }
        }

        // Revoke all sessions for the user, to update the roles.
        if (request.RevokeUserSessions)
            await RemoveBffSessionsAsync(userId, cancellationToken);

        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id, true));

        return _t["User Roles Updated Successfully."];
    }
}