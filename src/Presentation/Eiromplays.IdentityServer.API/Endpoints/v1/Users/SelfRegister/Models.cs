using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.SelfRegister;

public class Models
{
    public class Request : CreateUserRequest
    {

    }

    public class Response
    {
        public string? Message { get; set; }
    }
}