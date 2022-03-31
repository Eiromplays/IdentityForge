using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.GetUserRoles;

public class Models
{
    public class Request
    {
        public string? Id { get; set; }
    }
    
    public class Response
    {
        public List<UserRoleDto>? UserRolesDto { get; set; }
    }
}