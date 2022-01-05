namespace Eiromplays.IdentityServer.Infrastructure.Identity.Configurations;

public class DatabaseConfiguration
{
    public DatabaseConfiguration(){}

    public DatabaseConfiguration(bool useInMemoryDatabase,
        ConnectionStringsConfiguration connectionStringsConfiguration,
        DatabaseMigrationsConfiguration databaseMigrationsConfiguration,
        EncryptionKeysConfiguration encryptionKeysConfiguration)
    {
        UseInMemoryDatabase = useInMemoryDatabase;
        ConnectionStringsConfiguration = connectionStringsConfiguration;
        DatabaseMigrationsConfiguration = databaseMigrationsConfiguration;
        EncryptionKeysConfiguration = encryptionKeysConfiguration;
    }

    public bool UseInMemoryDatabase;

    public DatabaseMigrationsConfiguration? DatabaseMigrationsConfiguration { get; set; }

    public ConnectionStringsConfiguration? ConnectionStringsConfiguration { get; set; }

    public EncryptionKeysConfiguration? EncryptionKeysConfiguration { get; set; }
}