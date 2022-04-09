namespace Eiromplays.IdentityServer.Application.Identity.PersistedGrants;

public class GrantDto
{
    public string? SubjectId { get; set; }
    
    public string? ClientId { get; set; }

    public string? Description { get; set; }
    
    public IEnumerable<string> Scopes { get; set; } = new List<string>();
    
    public DateTime CreationTime { get; set; }
    
    public DateTime? Expiration { get; set; }
}