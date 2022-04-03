using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.GetProfile;

public class Models
{
    public class Request
    {
    }
    
    public class Response
    {
        public UserDetailsDto? UserDetails { get; set; }
    }
}