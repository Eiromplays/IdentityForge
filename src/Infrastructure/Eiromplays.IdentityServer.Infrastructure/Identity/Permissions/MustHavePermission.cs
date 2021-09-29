using Microsoft.AspNetCore.Authorization;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Permissions
{
    public class MustHavePermission : AuthorizeAttribute
    {
        public MustHavePermission(string permission)
        {
            Policy = permission;
        }
    }
}