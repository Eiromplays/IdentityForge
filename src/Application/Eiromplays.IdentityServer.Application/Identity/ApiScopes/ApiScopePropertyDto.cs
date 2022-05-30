namespace Eiromplays.IdentityServer.Application.Identity.ApiScopes;

public class ApiScopePropertyDto
{
    public int Id { get; set; }
    
    public string Key { get; set; } = default!;

    public string Value { get; set; } = default!;
    
    public int ScopeId { get; set; }
}