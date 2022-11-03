using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace Eiromplays.IdentityServer.Infrastructure;

public class CustomKeyVaultSecretManager : KeyVaultSecretManager
{
    public override string GetKey(KeyVaultSecret? secret)
    {
        // TODO: Add your own logic to map the secret to a configuration key.
        // Add option to specify a prefix to be removed from the secret name.
        return secret?.Name.Replace("EiromplaysIdentityServerAdmin-", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("--", ConfigurationPath.KeyDelimiter, StringComparison.OrdinalIgnoreCase) ?? string.Empty;
    }
}