using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.UpdateProfile;

public class Models
{
    public class Request
    {
        public UpdateUserRequest Data { get; set; } = default!;
    }
}