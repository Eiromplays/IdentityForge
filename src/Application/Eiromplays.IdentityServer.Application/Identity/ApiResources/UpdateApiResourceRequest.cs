namespace Eiromplays.IdentityServer.Application.Identity.ApiResources;

public class UpdateApiResourceRequest
{
    public int Id { get; set; }
    
    public string Name { get; set; } = default!;
    
    public string DisplayName { get; set; } = default!;
    public string Description { get; set; } = default!;
    
    public bool ShowInDiscoveryDocument { get; set; }
    public string AllowedAccessTokenSigningAlgorithms { get; set; } = default!;
    
    public bool Enabled { get; set; }
    
    public bool RequireResourceIndicator { get; set; }
    
    public bool NonEditable { get; set; }
}