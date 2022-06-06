namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;

public class LoginRequest
{
    public string Login { get; set; } = default!;
    public string Password { get; set; } = default!;

    public bool RememberMe { get; set; }
    
    public string? ReturnUrl { get; set; }
}

public class LoginRequestValidator : Validator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty()
            .MaximumLength(100);
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ReturnUrl)
            .MaximumLength(2000);
    }
}