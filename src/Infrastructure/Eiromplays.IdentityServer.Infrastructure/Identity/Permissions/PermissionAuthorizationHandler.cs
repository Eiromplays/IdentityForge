using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Permissions
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            /*if (await _permissionService.HasPermissionAsync(userId, requirement.Permission))
            {
                context.Succeed(requirement);
                await Task.CompletedTask;
            }*/

            return Task.CompletedTask;
        }
    }
}