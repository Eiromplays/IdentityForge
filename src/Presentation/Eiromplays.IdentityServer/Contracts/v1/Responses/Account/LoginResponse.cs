using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Contracts.v1.Responses.Account;

public class LoginResponse
{
    public string? Error { get; set; }
        
    public SignInResult? SignInResult { get; set; }
        
    public string? ValidReturnUrl { get; set; }
}