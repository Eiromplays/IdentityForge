using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.UpdateUser;

public class Models
{
    public class Request
    {
        public string Id { get; set; } = default!;
        public UpdateUserRequest UpdateUserRequest { get; set; } = default!;
    }
}