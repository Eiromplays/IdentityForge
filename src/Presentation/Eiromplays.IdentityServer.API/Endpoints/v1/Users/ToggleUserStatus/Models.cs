using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ToggleUserStatus;

public class Models
{
    public class Request : ToggleUserStatusRequest
    {
        public string? Id { get; set; }
    }
    
    public class Response
    {
        
    }
}