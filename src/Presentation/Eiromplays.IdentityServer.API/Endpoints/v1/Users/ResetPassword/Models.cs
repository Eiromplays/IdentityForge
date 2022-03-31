using Eiromplays.IdentityServer.Application.Identity.Users.Password;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ResetPassword;

public class Models
{
    public class Request
    {
        public ForgotPasswordRequest ForgotPasswordRequest { get; set; }
    }
    
    public class Response
    {
        public string? Message { get; set; }
    }
}