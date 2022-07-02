namespace Eiromplays.IdentityServer.Application.Identity.ApiResources;

public class ApiResourceClaimDto
{
    public int Id { get; set; }

    public string Type { get; set; } = default!;

    public int ApiResourceId { get; set; }
}