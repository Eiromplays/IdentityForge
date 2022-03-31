using Eiromplays.IdentityServer.Application.Identity.Roles;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.UpdatePermissions;

public class Models
{
    public class Request
    {
        public string? Id { get; set; }
        
        public UpdateRolePermissionsRequest UpdateRolePermissionsRequest { get; set; }
    }

    public class Response
    {
        public string? Message { get; set; }
    }
}