namespace Eiromplays.IdentityServer.Infrastructure.Identity.Configurations;

public class EncryptionKeysConfiguration
{
    public bool UseEncryption { get; set; }
    public string? IdentityEncryptionKey { get; set; }
    public string? IdentityEncryptionIv { get; set; }
}