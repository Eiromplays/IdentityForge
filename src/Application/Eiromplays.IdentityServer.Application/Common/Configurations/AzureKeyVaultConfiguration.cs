namespace Eiromplays.IdentityServer.Application.Common.Configurations;

public class AzureKeyVaultConfiguration
{
    public bool Enabled { get; set; } = false;

    public string KeyVaultUrl { get; set; } = default!;
}