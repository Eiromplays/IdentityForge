namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class CreateUserResponse
{
    public string UserId { get; set; } = default!;
    
    public string Message { get; set; } = default!;
    
    public CreateUserResponse(string userId, string message)
    {
        UserId = userId;
        Message = message;
    }
    
    public CreateUserResponse() { }
}