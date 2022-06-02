using Duende.IdentityServer.EntityFramework.Entities;

namespace Eiromplays.IdentityServer.Application.Identity.Resources;

public class ApiResourceDto
{
    public int Id { get; set; }
    public bool Enabled { get; set; } = true;
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public string? AllowedAccessTokenSigningAlgorithms { get; set; }
    public bool ShowInDiscoveryDocument { get; set; } = true;
    public bool RequireResourceIndicator { get; set; }
    public List<ApiResourceSecret> Secrets { get; set; } = new();
    public List<ApiResourceScope> Scopes { get; set; } = new();
    public List<ApiResourceClaim> UserClaims { get; set; } = new();
    public List<ApiResourceProperty> Properties { get; set; } = new();
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? Updated { get; set; }
    public DateTime? LastAccessed { get; set; }
    public bool NonEditable { get; set; }
}