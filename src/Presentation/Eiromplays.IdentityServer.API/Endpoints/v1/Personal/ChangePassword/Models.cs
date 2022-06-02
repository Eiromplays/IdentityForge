using Eiromplays.IdentityServer.Application.Identity.Users.Password;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.ChangePassword;

public class Models
{
    public class Request
    {
        public ChangePasswordRequest Data { get; set; } = default!;
    }
    
    public class Response
    {
        public string Message { get; set; } = default!;
    }
}