using Eiromplays.IdentityServer.Application.Identity.Users.Password;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ResetPassword;

public class Models
{
    public class Request
    {
        public ResetPasswordRequest Data { get; set; } = default!;
    }
    
    public class Response
    {
        public string? Message { get; set; }
    }
}