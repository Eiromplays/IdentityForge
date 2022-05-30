using Eiromplays.IdentityServer.Application.Identity.Roles;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.CreateOrUpdateRole;

public class Models
{
    public class Request
    {
        public CreateOrUpdateRoleRequest Data { get; set; } = default!;
    }

    public class Response
    {
        public string? Message { get; set; }
    }
}