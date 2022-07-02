namespace Eiromplays.IdentityServer.Application.Identity.ApiResources;

public class ApiResourceDto
{
    public int Id { get; set; }

    public bool Enabled { get; set; } = true;

    public string Name { get; set; } = default!;

    public string DisplayName { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string AllowedAccessTokenSigningAlgorithms { get; set; } = default!;

    public bool ShowInDiscoveryDocument { get; set; } = true;

    public bool RequireResourceIndicator { get; set; }

    public List<ApiResourceSecretDto> Secrets { get; set; } = new();

    public List<ApiResourceScopeDto> Scopes { get; set; } = new();

    public List<ApiResourceClaimDto> UserClaims { get; set; } = new();

    public List<ApiResourcePropertyDto> Properties { get; set; } = new();

    public DateTime Created { get; set; }

    public DateTime? Updated { get; set; }

    public DateTime? LastAccessed { get; set; }

    public bool NonEditable { get; set; }
}