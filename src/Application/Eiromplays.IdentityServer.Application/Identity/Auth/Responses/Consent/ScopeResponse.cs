namespace Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Consent;

public class ScopeResponse
{
    public string Value { get; set; } = default!;

    public string DisplayName { get; set; } = default!;

    public string Description { get; set; } = default!;

    public bool Emphasize { get; set; }

    public bool Required { get; set; }

    public bool Checked { get; set; }
}