using Eiromplays.IdentityServer.Application.Identity.Users.Password;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.SetPassword;

public class Models
{
    public class Request
    {
        public SetPasswordRequest Data { get; set; } = default!;
    }
    
    public class Response
    {
        public string Message { get; set; } = default!;
    }
}