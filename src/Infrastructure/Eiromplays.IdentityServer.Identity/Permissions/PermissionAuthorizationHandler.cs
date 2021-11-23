using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Eiromplays.IdentityServer.Identity.Permissions
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        public PermissionAuthorizationHandler()
        {
            
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            /*if (await _permissionService.HasPermissionAsync(userId, requirement.Permission))
            {
                context.Succeed(requirement);
                await Task.CompletedTask;
            }*/
        }
    }
}