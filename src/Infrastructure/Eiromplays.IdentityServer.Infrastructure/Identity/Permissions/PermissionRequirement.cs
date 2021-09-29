using Microsoft.AspNetCore.Authorization;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Permissions
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}