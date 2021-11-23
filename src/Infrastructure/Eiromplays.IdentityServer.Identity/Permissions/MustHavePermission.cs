using Microsoft.AspNetCore.Authorization;

namespace Eiromplays.IdentityServer.Identity.Permissions
{
    public class MustHavePermission : AuthorizeAttribute
    {
        public MustHavePermission(string permission)
        {
            Policy = permission;
        }
    }
}