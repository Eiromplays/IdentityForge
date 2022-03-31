using Eiromplays.IdentityServer.Application.Identity.Users.Password;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ForgotPassword;

public class Models
{
    public class Request
    {
        public ResetPasswordRequest ResetPasswordRequest { get; set; }
    }
    
    public class Response
    {
        public string? Message { get; set; }
    }
}