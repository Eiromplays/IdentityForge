using Eiromplays.IdentityServer.Application.Identity.Roles;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.CreateOrUpdateRole;

public class Models
{
    public class Request
    {
        public CreateOrUpdateRoleRequest? CreateOrUpdateRoleRequest { get; set; }
    }

    public class Response
    {
        public string? RoleId { get; set; }
    }
}