namespace Eiromplays.IdentityServer.Application.Identity.IdentityProviders;

public class IdentityProviderDto
{
    public int Id { get; set; }

    public string Scheme { get; set; } = default!;

    public string DisplayName { get; set; } = default!;

    public bool Enabled { get; set; }

    public string Type { get; set; } = default!;

    public Dictionary<string, string> Properties { get; set; } = new();

    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? Updated { get; set; }
    public DateTime? LastAccessed { get; set; }
    public bool NonEditable { get; set; }
}