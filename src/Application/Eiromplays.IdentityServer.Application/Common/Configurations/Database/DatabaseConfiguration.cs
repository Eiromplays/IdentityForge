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

    public DatabaseProvider DatabaseProvider { get; set; } = DatabaseProvider.PostgreSql;

    public bool ApplyDefaultSeeds { get; set; } = true;

    public bool ApplyDatabaseMigrations { get; set; } = true;

    public ConnectionStringsConfiguration ConnectionStringsConfiguration { get; set; } = new();

}