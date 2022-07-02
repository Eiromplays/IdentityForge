using Eiromplays.IdentityServer.Domain.Enums;
using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.InMemory;
using Hangfire.MySql;
using Hangfire.PostgreSql;
using Hangfire.SQLite;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Eiromplays.IdentityServer.Infrastructure.BackgroundJobs;

internal static class Startup
{
    private static readonly ILogger Logger = Log.ForContext(typeof(Startup));

    internal static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration config)
    {
        services.AddHangfireServer(options => config.GetSection("HangfireSettings:Server").Bind(options));

        services.AddHangfireConsoleExtensions();

        var storageSettings = config.GetSection("HangfireSettings:Storage").Get<HangfireStorageSettings>();

        if (string.IsNullOrEmpty(storageSettings.ConnectionString)) throw new Exception("Hangfire Storage Provider ConnectionString is not configured.");

        Logger.Information("Hangfire: Current Storage Provider : {StorageProvider}", storageSettings.StorageProvider);
        Logger.Information("For more Hangfire storage, visit https://www.hangfire.io/extensions.html");

        services.AddSingleton<JobActivator, EiaJobActivator>();

        services.AddHangfire((provider, hangfireConfig) => hangfireConfig
            .UseDatabase(storageSettings.StorageProvider, storageSettings.ConnectionString, config)
            .UseFilter(new EiaJobFilter(provider))
            .UseFilter(new LogJobFilter())
            .UseConsole());

        return services;
    }

    private static IGlobalConfiguration UseDatabase(this IGlobalConfiguration hangfireConfig, DatabaseProvider databaseProvider, string connectionString, IConfiguration config) =>
        databaseProvider switch
        {
            DatabaseProvider.PostgreSql =>
                hangfireConfig.UsePostgreSqlStorage(connectionString, config.GetSection("HangfireSettings:Storage:Options").Get<PostgreSqlStorageOptions>()),
            DatabaseProvider.SqlServer =>
                hangfireConfig.UseSqlServerStorage(connectionString, config.GetSection("HangfireSettings:Storage:Options").Get<SqlServerStorageOptions>()),
            DatabaseProvider.MySql =>
                hangfireConfig.UseStorage(new MySqlStorage(connectionString, config.GetSection("HangfireSettings:Storage:Options").Get<MySqlStorageOptions>())),
            DatabaseProvider.Sqlite => hangfireConfig.UseSQLiteStorage(connectionString, config.GetSection("HangfireSettings:Storage:Options").Get<SQLiteStorageOptions>()),
            DatabaseProvider.InMemory => hangfireConfig.UseInMemoryStorage(config.GetSection("HangfireSettings:Storage:Options").Get<InMemoryStorageOptions>()),
            _ => throw new Exception($"Hangfire Storage Provider {databaseProvider} is not supported.")
        };

    internal static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration config)
    {
        var dashboardOptions = config.GetSection("HangfireSettings:Dashboard").Get<DashboardOptions>();

        dashboardOptions.Authorization = new[]
        {
            new HangfireCustomBasicAuthenticationFilter
            {
                User = config.GetSection("HangfireSettings:Credentials:User").Value,
                Pass = config.GetSection("HangfireSettings:Credentials:Password").Value
            }
        };

        return app.UseHangfireDashboard(config["HangfireSettings:Route"], dashboardOptions);
    }
}