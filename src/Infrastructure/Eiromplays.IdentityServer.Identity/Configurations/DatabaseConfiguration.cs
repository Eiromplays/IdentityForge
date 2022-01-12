namespace Eiromplays.IdentityServer.Infrastructure.Identity.Configurations;

public class DatabaseConfiguration
{
    public DatabaseConfiguration(){}

    public DatabaseConfiguration(
        bool useInMemoryDatabase,
        bool applyDefaultSeeds,
        DatabaseMigrationsConfiguration databaseMigrationsConfiguration,
        ConnectionStringsConfiguration connectionStringsConfiguration)
    {
        UseInMemoryDatabase = useInMemoryDatabase;
        ApplyDefaultSeeds = applyDefaultSeeds;
        DatabaseMigrationsConfiguration = databaseMigrationsConfiguration;
        ConnectionStringsConfiguration = connectionStringsConfiguration;
    }

    public bool UseInMemoryDatabase { get; set; }

    public bool ApplyDefaultSeeds { get; set; }

    public DatabaseMigrationsConfiguration? DatabaseMigrationsConfiguration { get; set; }

    public ConnectionStringsConfiguration? ConnectionStringsConfiguration { get; set; }

}