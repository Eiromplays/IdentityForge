using Eiromplays.IdentityServer.Domain.Enums;

namespace Eiromplays.IdentityServer.Application.Common.Configurations.Database;

public class DatabaseConfiguration
{
    public DatabaseConfiguration(){}

    public DatabaseConfiguration(
        DatabaseProvider databaseProvider,
        bool applyDefaultSeeds,
        bool applyDatabaseMigrations,
        ConnectionStringsConfiguration connectionStringsConfiguration)
    {
        DatabaseProvider = databaseProvider;
        ApplyDefaultSeeds = applyDefaultSeeds;
        ApplyDatabaseMigrations = applyDatabaseMigrations;
        ConnectionStringsConfiguration = connectionStringsConfiguration;
    }

    public DatabaseProvider DatabaseProvider { get; set; }

    public bool ApplyDefaultSeeds { get; set; }

    public bool ApplyDatabaseMigrations { get; set; }

    public ConnectionStringsConfiguration? ConnectionStringsConfiguration { get; set; }

}