namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public class UpdateClientRequest
{
    public int Id { get; set; }
    public string? ClientName { get; set; }
    public string? Description { get; set; }
    
    public string? ClientUri { get; set; }
    public string? LogoUri { get; set; }
    
    public bool Enabled { get; set; }
    
    public bool RequireConsent { get; set; }
    
    public bool AllowRememberConsent { get; set; }
}