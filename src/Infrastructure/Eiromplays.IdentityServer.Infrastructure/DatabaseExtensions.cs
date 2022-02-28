using Duende.IdentityServer.EntityFramework.Storage;
using Eiromplays.IdentityServer.Application.Common.Configurations.Database;
using Eiromplays.IdentityServer.Application.Common.Configurations.Identity;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.Infrastructure.Persistence.DbContexts;
using Eiromplays.IdentityServer.Infrastructure.Persistence.DbContexts.Seeds;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Infrastructure;

public static class DatabaseExtensions
{
    public static void RegisterDbContexts(this IServiceCollection services,
        DatabaseConfiguration databaseConfiguration)
    {
        var migrationsAssembly = $"{typeof(DatabaseExtensions).Assembly.GetName().Name}.{databaseConfiguration.DatabaseProvider}";

        switch (databaseConfiguration.DatabaseProvider)
        {
            case DatabaseProvider.InMemory:
                services.RegisterInMemoryDbContexts();
                break;
            case DatabaseProvider.PostgreSql:
                services.RegisterNpgSqlDbContexts(databaseConfiguration, migrationsAssembly);
                break;
            case DatabaseProvider.SqlServer:
                services.RegisterSqlServerDbContexts(databaseConfiguration, migrationsAssembly);
                break;
            case DatabaseProvider.MySql:
                services.RegisterMySqlDbContexts(databaseConfiguration, migrationsAssembly);
                break;
            case DatabaseProvider.Sqlite:
                services.RegisterSqliteDbContexts(databaseConfiguration, migrationsAssembly);
                break;
            default:
                throw new ArgumentOutOfRangeException(databaseConfiguration.DatabaseProvider.ToString());
        }
    }

    private static void RegisterInMemoryDbContexts(this IServiceCollection services)
    {
        // Add Identity DbContext
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseInMemoryDatabase("EiromplaysIdentityServerDb"));
        
        // Add Configuration DbContext
        services.AddConfigurationDbContext<IdentityServerConfigurationDbContext>(options => options.ConfigureDbContext = sql =>
            sql.UseInMemoryDatabase("EiromplaysIdentityServerConfigurationDb"));
        
        // Add PersistedGrant DbContext
        services.AddOperationalDbContext<IdentityServerPersistedGrantDbContext>(options => options.ConfigureDbContext = sql =>
            sql.UseInMemoryDatabase("EiromplaysIdentityServerPersistedGrantDb"));
        
        // Add Data Protection DbContext
        services.AddDbContext<IdentityServerDataProtectionDbContext>(options =>
            options.UseInMemoryDatabase("EiromplaysIdentityServerDataProtectionDb"));
    }
    
    private static void RegisterNpgSqlDbContexts(this IServiceCollection services,
        DatabaseConfiguration databaseConfiguration, string? migrationsAssembly)
    {
        // Add Identity DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.IdentityDbConnection))
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseNpgsql(databaseConfiguration.ConnectionStringsConfiguration.IdentityDbConnection,
                    sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        // Add Configuration DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.ConfigurationDbConnection))
        {
            services.AddConfigurationDbContext<IdentityServerConfigurationDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseNpgsql(databaseConfiguration.ConnectionStringsConfiguration.ConfigurationDbConnection,
                        sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        // Add PersistedGrant DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration
                ?.PersistedGrantDbConnection))
        {
            services.AddOperationalDbContext<IdentityServerPersistedGrantDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseNpgsql(databaseConfiguration.ConnectionStringsConfiguration.PersistedGrantDbConnection,
                        sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        // Add Data Protection DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration
                ?.DataProtectionDbConnection))
        {
            services.AddDbContext<IdentityServerDataProtectionDbContext>(options =>
                options.UseNpgsql(databaseConfiguration.ConnectionStringsConfiguration.DataProtectionDbConnection,
                    sql => sql.MigrationsAssembly(migrationsAssembly)));
        }
    }
    
    private static void RegisterSqlServerDbContexts(this IServiceCollection services,
        DatabaseConfiguration databaseConfiguration, string? migrationsAssembly)
    {
        // Add Identity DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.IdentityDbConnection))
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(databaseConfiguration.ConnectionStringsConfiguration.IdentityDbConnection,
                    sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        // Add Configuration DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.ConfigurationDbConnection))
        {
            services.AddConfigurationDbContext<IdentityServerConfigurationDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(databaseConfiguration.ConnectionStringsConfiguration.ConfigurationDbConnection,
                        sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        // Add PersistedGrant DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration
                ?.PersistedGrantDbConnection))
        {
            services.AddOperationalDbContext<IdentityServerPersistedGrantDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(databaseConfiguration.ConnectionStringsConfiguration.PersistedGrantDbConnection,
                        sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        // Add Data Protection DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration
                ?.DataProtectionDbConnection))
        {
            services.AddDbContext<IdentityServerDataProtectionDbContext>(options =>
                options.UseSqlServer(databaseConfiguration.ConnectionStringsConfiguration.DataProtectionDbConnection,
                    sql => sql.MigrationsAssembly(migrationsAssembly)));
        }
    }
    
    private static void RegisterMySqlDbContexts(this IServiceCollection services,
        DatabaseConfiguration databaseConfiguration, string? migrationsAssembly)
    {
        // Add Identity DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.IdentityDbConnection))
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseMySql(databaseConfiguration.ConnectionStringsConfiguration.IdentityDbConnection,
                    databaseConfiguration.ConnectionStringsConfiguration
                        .IdentityDbConnection.GetMySqlServerVersion(),
                    sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        // Add Configuration DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.ConfigurationDbConnection))
        {
            services.AddConfigurationDbContext<IdentityServerConfigurationDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseMySql(databaseConfiguration.ConnectionStringsConfiguration.ConfigurationDbConnection,
                        databaseConfiguration.ConnectionStringsConfiguration
                            .ConfigurationDbConnection.GetMySqlServerVersion(),
            sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        // Add PersistedGrant DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration
                ?.PersistedGrantDbConnection))
        {
            services.AddOperationalDbContext<IdentityServerPersistedGrantDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseMySql(databaseConfiguration.ConnectionStringsConfiguration.PersistedGrantDbConnection,
                        databaseConfiguration.ConnectionStringsConfiguration
                            .PersistedGrantDbConnection.GetMySqlServerVersion(),
                        sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        // Add Data Protection DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration
                ?.DataProtectionDbConnection))
        {
            services.AddDbContext<IdentityServerDataProtectionDbContext>(options =>
                options.UseMySql(databaseConfiguration.ConnectionStringsConfiguration.DataProtectionDbConnection,
                    databaseConfiguration.ConnectionStringsConfiguration
                        .DataProtectionDbConnection.GetMySqlServerVersion(),
                    sql => sql.MigrationsAssembly(migrationsAssembly)));
        }
    }
    
    private static void RegisterSqliteDbContexts(this IServiceCollection services,
        DatabaseConfiguration databaseConfiguration, string? migrationsAssembly)
    {
        // Add Identity DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.IdentityDbConnection))
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlite(databaseConfiguration.ConnectionStringsConfiguration.IdentityDbConnection,
                    sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        // Add Configuration DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.ConfigurationDbConnection))
        {
            services.AddConfigurationDbContext<IdentityServerConfigurationDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseSqlite(databaseConfiguration.ConnectionStringsConfiguration.ConfigurationDbConnection,
                        sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        // Add PersistedGrant DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration
                ?.PersistedGrantDbConnection))
        {
            services.AddOperationalDbContext<IdentityServerPersistedGrantDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseSqlite(databaseConfiguration.ConnectionStringsConfiguration.PersistedGrantDbConnection,
                        sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        // Add Data Protection DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration
                ?.DataProtectionDbConnection))
        {
            services.AddDbContext<IdentityServerDataProtectionDbContext>(options =>
                options.UseSqlite(databaseConfiguration.ConnectionStringsConfiguration.DataProtectionDbConnection,
                    sql => sql.MigrationsAssembly(migrationsAssembly)));
        }
    }
    
    public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var databaseConfiguration =
            configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();

        if (!databaseConfiguration.ApplyDatabaseMigrations)
            return;

        await using var scope = serviceProvider.CreateAsyncScope();

        var services = scope.ServiceProvider;
        try
        {
            await using var identityDbContext = services.GetRequiredService<IdentityDbContext>();
            await identityDbContext.Database.MigrateAsync();

            await using var identityServerConfigurationDbContext =
                services.GetRequiredService<IdentityServerConfigurationDbContext>();
            await identityServerConfigurationDbContext.Database.MigrateAsync();

            await using var identityServerPersistedGrantDbContext =
                services.GetRequiredService<IdentityServerPersistedGrantDbContext>();
            await identityServerPersistedGrantDbContext.Database.MigrateAsync();

            await using var identityServerDataProtectionDbContext =
                services.GetRequiredService<IdentityServerDataProtectionDbContext>();
            await identityServerDataProtectionDbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();

            logger.LogError(ex, "An error occurred while migrating or seeding the database(s)");

            throw;
        }
    }

    public static async Task ApplySeedsAsync(this IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var databaseConfiguration =
            configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();

        if (!databaseConfiguration.ApplyDefaultSeeds)
            return;

        await using var scope = serviceProvider.CreateAsyncScope();

        var services = scope.ServiceProvider;
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        var identityData = services.GetRequiredService<IdentityData>();

        await IdentityDbContextSeed.SeedDefaultUsersAndRolesAsync(userManager, roleManager, identityData);

        var identityServerDataConfiguration = scope.ServiceProvider.GetRequiredService<IdentityServerData>();
        var identityServerConfigurationDbContext = services.GetRequiredService<IdentityServerConfigurationDbContext>();

        await IdentityServerConfigurationDbContextSeed.SeedIdentityServerDataAsync(
            identityServerConfigurationDbContext, identityServerDataConfiguration);
    }

    private static ServerVersion GetMySqlServerVersion(this string? connectionString)
    {
        try
        {
            return ServerVersion.AutoDetect(connectionString);
        }
        catch (Exception)
        {
            Console.WriteLine("Could not detect MySQL server version. Using latest supported version.");

            return MySqlServerVersion.LatestSupportedServerVersion;
        }
    }
}