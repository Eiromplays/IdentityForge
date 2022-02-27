using Eiromplays.IdentityServer.Domain.Enums;

namespace Eiromplays.IdentityServer.Application.Common.Configurations.Database;

public class DatabaseConfiguration
{
    public DatabaseConfiguration(){}

    public DatabaseConfiguration(
        DatabaseProvider databaseProvider,
        bool applyDefaultSeeds,
        DatabaseMigrationsConfiguration databaseMigrationsConfiguration,
        ConnectionStringsConfiguration connectionStringsConfiguration)
    {
        DatabaseProvider = databaseProvider;
        ApplyDefaultSeeds = applyDefaultSeeds;
        DatabaseMigrationsConfiguration = databaseMigrationsConfiguration;
        ConnectionStringsConfiguration = connectionStringsConfiguration;
    }

    public DatabaseProvider DatabaseProvider { get; set; }
    
    public bool ApplyDefaultSeeds { get; set; }

    public DatabaseMigrationsConfiguration? DatabaseMigrationsConfiguration { get; set; }

    public ConnectionStringsConfiguration? ConnectionStringsConfiguration { get; set; }

}