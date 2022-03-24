using Duende.Bff.EntityFramework;
using Eiromplays.IdentityServer.Application.Common.Configurations.Database;
using Eiromplays.IdentityServer.Domain.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Infrastructure.BFF;

public static class DatabaseExtensions
{
    public static void RegisterDbContexts(this BffBuilder bffBuilder,
        DatabaseConfiguration databaseConfiguration)
    {
        var migrationsAssembly = $"{typeof(Infrastructure.DatabaseExtensions).Assembly.GetName().Name}.{databaseConfiguration.DatabaseProvider}";

        switch (databaseConfiguration.DatabaseProvider)
        {
            case DatabaseProvider.InMemory:
                bffBuilder.RegisterInMemoryDbContexts();
                break;
            case DatabaseProvider.PostgreSql:
                bffBuilder.RegisterNpgSqlDbContexts(databaseConfiguration, migrationsAssembly);
                break;
            case DatabaseProvider.SqlServer:
                bffBuilder.RegisterSqlServerDbContexts(databaseConfiguration, migrationsAssembly);
                break;
            case DatabaseProvider.MySql:
                bffBuilder.RegisterMySqlDbContexts(databaseConfiguration, migrationsAssembly);
                break;
            case DatabaseProvider.Sqlite:
                bffBuilder.RegisterSqliteDbContexts(databaseConfiguration, migrationsAssembly);
                break;
            default:
                throw new ArgumentOutOfRangeException(databaseConfiguration.DatabaseProvider.ToString());
        }
    }

    private static void RegisterInMemoryDbContexts(this BffBuilder bffBuilder)
    {
        // Add Session DbContext
        bffBuilder.AddEntityFrameworkServerSideSessions(options =>
            options.UseInMemoryDatabase("EiromplaysIdentityServerDb"));
    }
    
    private static void RegisterNpgSqlDbContexts(this BffBuilder bffBuilder,
        DatabaseConfiguration databaseConfiguration, string? migrationsAssembly)
    {
        // Add Session DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration.SessionDbConnection))
        {
            bffBuilder.AddEntityFrameworkServerSideSessions(options =>
                options.UseNpgsql(databaseConfiguration.ConnectionStringsConfiguration.SessionDbConnection,
                    sql => sql.MigrationsAssembly(migrationsAssembly)));
        }
    }
    
    private static void RegisterSqlServerDbContexts(this BffBuilder bffBuilder,
        DatabaseConfiguration databaseConfiguration, string? migrationsAssembly)
    {
        // Add Session DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration.SessionDbConnection))
        {
            bffBuilder.AddEntityFrameworkServerSideSessions(options =>
                options.UseSqlServer(databaseConfiguration.ConnectionStringsConfiguration.SessionDbConnection,
                    sql => sql.MigrationsAssembly(migrationsAssembly)));
        }
    }
    
    private static void RegisterMySqlDbContexts(this BffBuilder bffBuilder,
        DatabaseConfiguration databaseConfiguration, string? migrationsAssembly)
    {
        // Add Session DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration.SessionDbConnection))
        {
            bffBuilder.AddEntityFrameworkServerSideSessions(options =>
                options.UseMySql(databaseConfiguration.ConnectionStringsConfiguration.SessionDbConnection,
                    databaseConfiguration.ConnectionStringsConfiguration
                        .IdentityDbConnection.GetMySqlServerVersion(),
                    sql => sql.MigrationsAssembly(migrationsAssembly)));
        }
    }
    
    private static void RegisterSqliteDbContexts(this BffBuilder bffBuilder,
        DatabaseConfiguration databaseConfiguration, string? migrationsAssembly)
    {
        // Add Session DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration.SessionDbConnection))
        {
            bffBuilder.AddEntityFrameworkServerSideSessions(options =>
                options.UseSqlite(databaseConfiguration.ConnectionStringsConfiguration.SessionDbConnection,
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
            await using var sessionDbContext = services.GetRequiredService<SessionDbContext>();
            await sessionDbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();

            logger.LogError(ex, "An error occurred while migrating or seeding the database(s)");

            throw;
        }
    }
}