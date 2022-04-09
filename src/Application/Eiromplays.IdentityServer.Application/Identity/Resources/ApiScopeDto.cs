using Duende.IdentityServer.EntityFramework.Entities;

namespace Eiromplays.IdentityServer.Application.Identity.Resources;

public class ApiScopeDto
{
    public int Id { get; set; }
    public bool Enabled { get; set; } = true;
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public bool Required { get; set; }
    public bool Emphasize { get; set; }
    public bool ShowInDiscoveryDocument { get; set; } = true;
    public List<ApiScopeClaim> UserClaims { get; set; } = new();
    public List<ApiScopeProperty> Properties { get; set; } = new();
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? Updated { get; set; }
    public DateTime? LastAccessed { get; set; }
    public bool NonEditable { get; set; }
}