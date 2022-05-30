namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public class ClientClaimDto
{
    public int Id { get; set; }
    
    public string Type { get; set; } = default!;
    
    public string Value { get; set; } = default!;
}