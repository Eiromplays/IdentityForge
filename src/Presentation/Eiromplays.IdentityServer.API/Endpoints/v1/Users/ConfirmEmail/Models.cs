namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ConfirmEmail;

public class Models
{
    public class Request
    {
        [QueryParam] public string? UserId { get; set; } = default;

        [QueryParam] public string? Code { get; set; } = default;

        [QueryParam] public string? ReturnUrl { get; set; } = default;
    }
    
    public class Response
    {
        public string? Message { get; set; }
    }
}