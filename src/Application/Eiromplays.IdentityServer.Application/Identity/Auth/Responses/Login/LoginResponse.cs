using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;

public class LoginResponse
{
    public string? Error { get; set; }
        
    public SignInResult? SignInResult { get; set; }
        
    public string? ValidReturnUrl { get; set; }
    
    public string? TwoFactorReturnUrl { get; set; }
    
    public string? ExternalLoginReturnUrl { get; set; }
    
    public string? Message { get; set; }
}