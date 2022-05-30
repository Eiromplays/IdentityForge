namespace Eiromplays.IdentityServer.Application.Identity.ApiResources;

public class ApiResourceScopeDto
{
    public int Id { get; set; }
    public string Scope { get; set; } = default!;

    public int ApiResourceId { get; set; }
}