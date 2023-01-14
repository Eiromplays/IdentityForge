namespace Eiromplays.IdentityServer.Application.Identity.IdentityProviders;

public class CreateIdentityProviderRequest
{
    public string Scheme { get; set; } = default!;

    public string DisplayName { get; set; } = default!;

    public bool Enabled { get; set; }

    public string Type { get; set; } = default!;

    public Dictionary<string, string> Properties { get; set; } = new();
}