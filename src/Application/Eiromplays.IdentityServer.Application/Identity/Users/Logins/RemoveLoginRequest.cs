namespace Eiromplays.IdentityServer.Application.Identity.Users.Logins;

public class RemoveLoginRequest
{
    public string LoginProvider { get; set; } = default!;
    
    public string ProviderKey { get; set; } = default!;
}

public class RemoveLoginRequestValidator : CustomValidator<RemoveLoginRequest>
{
    public RemoveLoginRequestValidator()
    {
        RuleFor(p => p.LoginProvider)
            .NotEmpty();

        RuleFor(p => p.ProviderKey)
            .NotEmpty();
    }
}