using Eiromplays.IdentityServer.Application.Identity.Users.Password;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ForgotPassword;

public class Models
{
    public class Request
    {
        public ForgotPasswordRequest Data { get; set; } = default!;
    }
    
    public class Response
    {
        public string? Message { get; set; }
    }
}