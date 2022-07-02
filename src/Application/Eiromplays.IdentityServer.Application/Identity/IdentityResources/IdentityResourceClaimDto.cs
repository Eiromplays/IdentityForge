namespace Eiromplays.IdentityServer.Application.Identity.IdentityResources;

public class IdentityResourceClaimDto
{
    public int Id { get; set; }

    public string Type { get; set; } = default!;

    public int IdentityResourceId { get; set; }
}