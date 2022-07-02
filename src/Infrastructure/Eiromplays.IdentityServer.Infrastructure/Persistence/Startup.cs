using Duende.Bff.EntityFramework;
using Duende.IdentityServer.EntityFramework.Storage;
using Eiromplays.IdentityServer.Application.Common.Configurations.Database;
using Eiromplays.IdentityServer.Application.Common.Persistence;
using Eiromplays.IdentityServer.Domain.Common.Contracts;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure.Common;
using Eiromplays.IdentityServer.Infrastructure.Persistence.ConnectionString;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Initialization;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Serilog;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence;

internal static class Startup
{
    private static readonly ILogger Logger = Log.ForContext(typeof(Startup));

    internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config, ProjectType projectType)
    {
        if (projectType is ProjectType.Spa) return services;

        var databaseConfiguration = config.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();

        Logger.Information("Current DB Provider : {DatabaseProvider}", databaseConfiguration.DatabaseProvider);

        return services
            .Configure<DatabaseConfiguration>(config.GetSection(nameof(DatabaseConfiguration)))

            .AddDbContext<ApplicationDbContext>(m => m.UseDatabase(
                databaseConfiguration.DatabaseProvider,
                databaseConfiguration.ConnectionStringsConfiguration.ApplicationDbConnection))
            .AddConfigurationDbContext<ApplicationDbContext>()
            .AddOperationalDbContext<ApplicationDbContext>()
            .AddDbContext<SessionDbContext>(m => m.UseDatabase(
                databaseConfiguration.DatabaseProvider,
                databaseConfiguration.ConnectionStringsConfiguration.SessionDbConnection))

            .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
            .AddTransient<ApplicationDbInitializer>()
            .AddTransient<SessionDbInitializer>()
            .AddTransient<ApplicationDbSeeder>()

            .AddServices(typeof(ICustomSeeder), ServiceLifetime.Transient, projectType)
            .AddTransient<CustomSeederRunner>()

            .AddTransient<IConnectionStringSecurer, ConnectionStringSecurer>()
            .AddTransient<IConnectionStringValidator, ConnectionStringValidator>()

            .AddRepositories();
    }

    internal static BffBuilder AddBffPersistence(this BffBuilder bffBuilder, IConfiguration configuration, ProjectType projectType)
    {
        if (projectType is not ProjectType.Spa)
            return bffBuilder;

        var databaseConfiguration = configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();

        Logger.Information("Current DB Provider : {DatabaseProvider}", databaseConfiguration.DatabaseProvider);

        bffBuilder.AddEntityFrameworkServerSideSessions(options => options.UseDatabase(
            databaseConfiguration.DatabaseProvider,
            databaseConfiguration.ConnectionStringsConfiguration.SessionDbConnection));

        bffBuilder.Services
            .Configure<DatabaseConfiguration>(configuration.GetSection(nameof(DatabaseConfiguration)))
            .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
            .AddTransient<SessionDbInitializer>()
            .AddTransient<IConnectionStringSecurer, ConnectionStringSecurer>()
            .AddTransient<IConnectionStringValidator, ConnectionStringValidator>();

        return bffBuilder;
    }

    // ReSharper disable once UnusedMethodReturnValue.Local
    private static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, DatabaseProvider databaseProvider, string connectionString)
    {
        switch (databaseProvider)
        {
            case DatabaseProvider.PostgreSql:
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                return builder.UseNpgsql(connectionString, e => e.MigrationsAssembly("Migrators.PostgreSql"));

            case DatabaseProvider.SqlServer:
                return builder.UseSqlServer(connectionString, e =>
                     e.MigrationsAssembly("Migrators.SqlServer"));

            case DatabaseProvider.MySql:
                return builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), e =>
                     e.MigrationsAssembly("Migrators.MySql")
                      .SchemaBehavior(MySqlSchemaBehavior.Ignore));

            case DatabaseProvider.Sqlite:
                return builder.UseSqlite(connectionString, e => e.MigrationsAssembly("Migrators.Sqlite"));

            case DatabaseProvider.InMemory:
                return builder.UseInMemoryDatabase(connectionString);

            default:
                throw new InvalidOperationException($"DB Provider {databaseProvider} is not supported.");
        }
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Add Repositories
        services.AddScoped(typeof(IRepository<>), typeof(ApplicationDbRepository<>));

        foreach (var aggregateRootType in
            typeof(IAggregateRoot).Assembly.GetExportedTypes()
                .Where(t => typeof(IAggregateRoot).IsAssignableFrom(t) && t.IsClass)
                .ToList())
        {
            // Add ReadRepositories.
            services.AddScoped(typeof(IReadRepository<>).MakeGenericType(aggregateRootType), sp =>
                sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregateRootType)));

            // Decorate the repositories with EventAddingRepositoryDecorators and expose them as IRepositoryWithEvents.
            services.AddScoped(typeof(IRepositoryWithEvents<>).MakeGenericType(aggregateRootType), sp =>
                Activator.CreateInstance(
                    typeof(EventAddingRepositoryDecorator<>).MakeGenericType(aggregateRootType),
                    sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregateRootType)))
                ?? throw new InvalidOperationException($"Couldn't create EventAddingRepositoryDecorator for aggregateRootType {aggregateRootType.Name}"));
        }

        return services;
    }
}