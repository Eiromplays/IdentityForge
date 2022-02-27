namespace Eiromplays.IdentityServer.Application.Common.Configurations.Database;

public class DatabaseMigrationsConfiguration
{
    public bool ApplyDatabaseMigrations { get; set; } = false;

    public string? IdentityDbMigrationsAssembly { get; set; }

    public string? ConfigurationDbMigrationsAssembly { get; set; }

    public string? PersistedGrantDbMigrationsAssembly { get; set; }

    public string? DataProtectionDbMigrationsAssembly { get; set; }

    public string? PermissionDbMigrationsAssembly { get; set; }

    public void SetMigrationsAssemblies(string? commonMigrationsAssembly)
    {
        IdentityDbMigrationsAssembly = commonMigrationsAssembly;
        ConfigurationDbMigrationsAssembly = commonMigrationsAssembly;
        DataProtectionDbMigrationsAssembly = commonMigrationsAssembly;
        PersistedGrantDbMigrationsAssembly = commonMigrationsAssembly;
        PermissionDbMigrationsAssembly = commonMigrationsAssembly;
    }
}