using Duende.Bff.EntityFramework;
using Eiromplays.IdentityServer.Application.Common.Configurations.Database;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Serilog;

namespace Eiromplays.IdentityServer.Infrastructure;

internal static class Startup
{
    private static readonly ILogger _logger = Log.ForContext(typeof(Startup));
    
    internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConfiguration =
            configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();

        var dbProvider = databaseConfiguration.DatabaseProvider;
        
        _logger.Information("Current DB Provider : {DBProvider}", dbProvider);

        return services
            .AddDbContexts(databaseConfiguration);
    }

    internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, DatabaseProvider dbProvider, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string is empty");
        }
        
        switch (dbProvider)
        {
            case DatabaseProvider.InMemory:
                return builder.UseInMemoryDatabase(connectionString);
            
            case DatabaseProvider.PostgreSql:
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                return builder.UseNpgsql(connectionString, e =>
                    e.MigrationsAssembly("Migrators.PostgreSQL"));

            case DatabaseProvider.SqlServer:
                return builder.UseSqlServer(connectionString, e =>
                    e.MigrationsAssembly("Migrators.SqlServer"));

            case DatabaseProvider.MySql:
                return builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), e =>
                    e.MigrationsAssembly("Migrators.MySQL")
                        .SchemaBehavior(MySqlSchemaBehavior.Ignore));

            case DatabaseProvider.Sqlite:
                return builder.UseSqlite(connectionString, e =>
                    e.MigrationsAssembly("Migrators.Sqlite"));
            
            default:
                throw new InvalidOperationException($"DB Provider {dbProvider} is not supported.");
        }
    }
    
    internal static IServiceCollection AddDbContexts(this IServiceCollection services,
        DatabaseConfiguration databaseConfiguration, BffBuilder? bffBuilder = null)
    {
        // Add Session DbContext
        bffBuilder.AddEntityFrameworkServerSideSessions(options =>
            options.UseDatabase(databaseConfiguration.DatabaseProvider, databaseConfiguration.ConnectionStringsConfiguration.SessionDbConnection));

        return services
            .AddDbContext<IdentityDbContext>(options => options.UseDatabase(databaseConfiguration.DatabaseProvider,
                databaseConfiguration.ConnectionStringsConfiguration.IdentityDbConnection))
            
            .AddDbContext<IdentityServerConfigurationDbContext>(options =>
                options.UseDatabase(databaseConfiguration.DatabaseProvider,
                    databaseConfiguration.ConnectionStringsConfiguration.ConfigurationDbConnection))
            
            .AddDbContext<IdentityServerPersistedGrantDbContext>(options =>
                options.UseDatabase(databaseConfiguration.DatabaseProvider,
                    databaseConfiguration.ConnectionStringsConfiguration.PersistedGrantDbConnection))
            
            .AddDbContext<IdentityServerDataProtectionDbContext>(options =>
                options.UseDatabase(databaseConfiguration.DatabaseProvider,
                    databaseConfiguration.ConnectionStringsConfiguration.DataProtectionDbConnection));
    }
}