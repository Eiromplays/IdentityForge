using Eiromplays.IdentityServer.Application.Common.Configurations;

namespace Eiromplays.IdentityServer.Application.Common;

public class SecretsConfiguration
{
    public AwsSecretsManagerConfiguration? AwsSecretsManagerConfiguration { get; set; }

    public AzureKeyVaultConfiguration? AzureKeyVaultConfiguration { get; set; }
}