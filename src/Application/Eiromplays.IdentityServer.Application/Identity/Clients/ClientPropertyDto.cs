namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public class ClientPropertyDto
{
    public int Id { get; set; }
    
    public string Key { get; set; } = default!;
    
    public string Value { get; set; } = default!;
}