using Microsoft.AspNetCore.Authorization;
using Shared.Authorization;

namespace Eiromplays.IdentityServer.Infrastructure.Auth.Permissions;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = EIAPermission.NameFor(action, resource);
}