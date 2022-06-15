
namespace Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Consent;

public class ProcessConsentResponse
{
    public bool IsRedirect => RedirectUri is not null;
    public string? RedirectUri { get; set; }
    
    public Duende.IdentityServer.Models.Client? Client { get; set; }
    
    public bool ShowResponse => Response is not null;
    public ConsentResponse? Response { get; set; }
    
    public bool HasValidationError => ValidationError is not null;
    public string? ValidationError { get; set; }
}