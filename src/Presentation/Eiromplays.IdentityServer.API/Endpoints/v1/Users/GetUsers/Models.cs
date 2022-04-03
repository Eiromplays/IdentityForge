using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.GetUsers;

public class Models
{
    public class Response
    {
        public List<UserDetailsDto>? Users { get; set; }
    }
}