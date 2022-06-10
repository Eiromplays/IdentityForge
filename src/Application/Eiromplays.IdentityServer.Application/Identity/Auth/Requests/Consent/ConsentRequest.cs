namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Consent;

public class ConsentRequest
{
    public bool Deny { get; set; }
    
    public bool Remember { get; set; }

    public string ReturnUrl { get; set; } = default!;
}

public class ConsentRequestValidator : Validator<ConsentRequest>
{
    public ConsentRequestValidator()
    {
        RuleFor(x => x.ReturnUrl)
            .NotEmpty()
            .MaximumLength(2000);
    }
}