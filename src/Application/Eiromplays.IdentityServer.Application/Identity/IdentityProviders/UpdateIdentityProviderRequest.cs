namespace Eiromplays.IdentityServer.Application.Identity.IdentityProviders;

public class UpdateIdentityProviderRequest
{
    public int Id { get; set; }

    public string? Scheme { get; set; }

    public string? DisplayName { get; set; }

    public bool? Enabled { get; set; }

    public string? Type { get; set; }

    public Dictionary<string, string> Properties { get; set; } = new();
}