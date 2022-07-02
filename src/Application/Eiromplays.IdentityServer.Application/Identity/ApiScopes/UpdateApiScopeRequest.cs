namespace Eiromplays.IdentityServer.Application.Identity.ApiScopes;

public class UpdateApiScopeRequest
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string DisplayName { get; set; } = default!;
    public string Description { get; set; } = default!;

    public bool ShowInDiscoveryDocument { get; set; }
    public bool Emphasize { get; set; }

    public bool Enabled { get; set; }

    public bool Required { get; set; }

    public bool NonEditable { get; set; }
}