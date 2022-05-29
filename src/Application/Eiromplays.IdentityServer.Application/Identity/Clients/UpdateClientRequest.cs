namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public class UpdateClientRequest
{
    public string Id { get; set; } = default!;
    public string? ClientName { get; set; }
    public string? Description { get; set; }
}