namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ConfirmPhoneNumber;

public class Models
{
    public class Request
    {
        [QueryParam]
        public string? UserId { get; set; }
        
        [QueryParam]
        public string? Code { get; set; }
    }
    
    public class Response
    {
        public string? Message { get; set; }
    }
}