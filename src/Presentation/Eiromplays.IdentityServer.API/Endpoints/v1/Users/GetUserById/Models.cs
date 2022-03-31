using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.GetUserById;

public class Models
{
    public class Request
    {
        public string? Id { get; set; }
    }
    
    public class Response
    {
        public UserDetailsDto? UserDetails { get; set; }
    }
}