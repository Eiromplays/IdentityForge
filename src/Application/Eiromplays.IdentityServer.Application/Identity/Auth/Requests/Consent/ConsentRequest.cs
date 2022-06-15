namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Consent;

public class ConsentRequest
{
    public string Button { get; set; } = default!;
    
    public IEnumerable<string> ScopesConsented { get; set; } = Enumerable.Empty<string>();
    
    public bool RememberConsent { get; set; }
    
    public string? ReturnUrl { get; set; } = default!;
    
    public string? Description { get; set; } = default!;
}

public class ConsentRequestRequestValidator : Validator<ConsentRequest>
{
    public ConsentRequestRequestValidator()
    {
        RuleFor(x => x.ReturnUrl).NotEmpty();
        RuleFor(x => x.Button).NotEmpty();
    }
}