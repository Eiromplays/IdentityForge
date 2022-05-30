namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public class UpdateClientRequest
{
    public int Id { get; set; }
    
    public string ClientId { get; set; } = default!;
    
    public string ClientName { get; set; } = default!;
    public string Description { get; set; } = default!;
    
    public string ClientUri { get; set; } = default!;
    public string LogoUri { get; set; } = default!;
    
    public bool Enabled { get; set; }
    
    public bool RequireConsent { get; set; }
    
    public bool AllowRememberConsent { get; set; }
}