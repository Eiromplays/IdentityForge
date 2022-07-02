using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Consent;

namespace Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Consent;

public class ConsentResponse : ConsentRequest
{
    public string ClientName { get; set; } = default!;

    public string ClientUrl { get; set; } = default!;

    public string ClientLogoUrl { get; set; } = default!;

    public bool AllowRememberConsent { get; set; }

    public IEnumerable<ScopeResponse> IdentityScopes { get; set; } = Enumerable.Empty<ScopeResponse>();

    public IEnumerable<ScopeResponse> ApiScopes { get; set; } = Enumerable.Empty<ScopeResponse>();
}