using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.UpdateUserRoles;

public class Models
{
    public class Request : UserRolesRequest
    {
        public string? Id { get; set; }
    }
    
    public class Response
    {
        public string? Message { get; set; }
    }
}