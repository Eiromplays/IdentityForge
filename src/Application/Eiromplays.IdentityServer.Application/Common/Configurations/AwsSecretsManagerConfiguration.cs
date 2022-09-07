namespace Eiromplays.IdentityServer.Application.Common.Configurations;

public class AwsSecretsManagerConfiguration
{
    public bool Enabled { get; set; } = false;

    public List<string> AllowedPrefixes { get; set; } = new();
}