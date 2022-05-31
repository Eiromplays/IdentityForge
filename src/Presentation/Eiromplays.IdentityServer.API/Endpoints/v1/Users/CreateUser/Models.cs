using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.CreateUser;

public class Models
{
    public class Request
    {
        public CreateUserRequest Data { get; set; } = default!;
    }
}