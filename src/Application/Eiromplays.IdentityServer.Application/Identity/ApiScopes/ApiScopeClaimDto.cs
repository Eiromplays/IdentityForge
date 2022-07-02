namespace Eiromplays.IdentityServer.Application.Identity.ApiScopes;

public class ApiScopeClaimDto
{
    public int Id { get; set; }

    public string Type { get; set; } = default!;

    public int ScopeId { get; set; }
}