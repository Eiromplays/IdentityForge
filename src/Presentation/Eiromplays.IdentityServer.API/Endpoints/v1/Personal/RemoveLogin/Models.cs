using Eiromplays.IdentityServer.Application.Identity.Users.Logins;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.RemoveLogin;

public class Models
{
    public class Request
    {
        public RemoveLoginRequest Data { get; set; } = default!;
    }
    
    public class Response
    {
        public string Message { get; set; } = default!;
    }
}