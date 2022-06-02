namespace Eiromplays.IdentityServer.Application.Identity.ApiScopes;

public class ApiScopeDto
{
    public int Id { get; set; }

    public bool Enabled { get; set; } = true;
    
    public string Name { get; set; } = default!;
    
    public string DisplayName { get; set; } = default!;

    public string Description { get; set; } = default!;
    
    public bool Required { get; set; }
    
    public bool Emphasize { get; set; }
    
    public bool ShowInDiscoveryDocument { get; set; } = true;

    public List<ApiScopeClaimDto> UserClaims { get; set; } = new();
    
    public List<ApiScopePropertyDto> Properties { get; set; } = new();
    
    public DateTime Created { get; set; }
    
    public DateTime? Updated { get; set; }
    
    public DateTime? LastAccessed { get; set; }
    
    public bool NonEditable { get; set; }
}