using System.Reflection;
using Duende.IdentityServer.EntityFramework.Storage;
using Eiromplays.IdentityServer.Application.Common.Configurations.Database;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eiromplays.IdentityServer.Infrastructure;

public static class DatabaseExtensions
{
    public static void RegisterDbContexts(this IServiceCollection services,
        DatabaseConfiguration databaseConfiguration)
    {
        var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;
        
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
        
        // Add Permission DbContext
        services.AddDbContext<PermissionDbContext>(options =>
            options.UseInMemoryDatabase("EiromplaysPermissionDb"));
    }
    
    private static void RegisterNpgSqlDbContexts(this IServiceCollection services,
        DatabaseConfiguration databaseConfiguration, string? migrationsAssembly)
    {
        // Add Identity DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.IdentityDbConnection))
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseNpgsql(databaseConfiguration.ConnectionStringsConfiguration.IdentityDbConnection,
                    sql => sql.MigrationsAssembly(
                        databaseConfiguration.DatabaseMigrationsConfiguration?.IdentityDbMigrationsAssembly ??
                        migrationsAssembly)));
        }

        // Add Configuration DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.ConfigurationDbConnection))
        {
            services.AddConfigurationDbContext<IdentityServerConfigurationDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseNpgsql(databaseConfiguration.ConnectionStringsConfiguration.ConfigurationDbConnection,
                        sql => sql.MigrationsAssembly(
                            databaseConfiguration.DatabaseMigrationsConfiguration?.ConfigurationDbMigrationsAssembly ??
                            migrationsAssembly)));
        }

        // Add PersistedGrant DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration
                ?.PersistedGrantDbConnection))
        {
            services.AddOperationalDbContext<IdentityServerPersistedGrantDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseNpgsql(databaseConfiguration.ConnectionStringsConfiguration.PersistedGrantDbConnection,
                        sql => sql.MigrationsAssembly(
                            databaseConfiguration.DatabaseMigrationsConfiguration?.PersistedGrantDbMigrationsAssembly ??
                            migrationsAssembly)));
        }

        // Add Data Protection DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration
                ?.DataProtectionDbConnection))
        {
            services.AddDbContext<IdentityServerDataProtectionDbContext>(options =>
                options.UseNpgsql(databaseConfiguration.ConnectionStringsConfiguration.DataProtectionDbConnection,
                    sql => sql.MigrationsAssembly(
                        databaseConfiguration.DatabaseMigrationsConfiguration?.DataProtectionDbMigrationsAssembly ??
                        migrationsAssembly)));
        }

        // Add Permission DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.PermissionDbConnection))
        {
            services.AddDbContext<PermissionDbContext>(options =>
                options.UseNpgsql(databaseConfiguration.ConnectionStringsConfiguration.PermissionDbConnection,
                    sql => sql.MigrationsAssembly(
                        databaseConfiguration.DatabaseMigrationsConfiguration?.PermissionDbMigrationsAssembly ??
                        migrationsAssembly)));

            services.AddScoped<IPermissionDbContext>(provider => provider.GetRequiredService<PermissionDbContext>());
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
                    sql => sql.MigrationsAssembly(
                        databaseConfiguration.DatabaseMigrationsConfiguration?.IdentityDbMigrationsAssembly ??
                        migrationsAssembly)));
        }

        // Add Configuration DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.ConfigurationDbConnection))
        {
            services.AddConfigurationDbContext<IdentityServerConfigurationDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(databaseConfiguration.ConnectionStringsConfiguration.ConfigurationDbConnection,
                        sql => sql.MigrationsAssembly(
                            databaseConfiguration.DatabaseMigrationsConfiguration?.ConfigurationDbMigrationsAssembly ??
                            migrationsAssembly)));
        }

        // Add PersistedGrant DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration
                ?.PersistedGrantDbConnection))
        {
            services.AddOperationalDbContext<IdentityServerPersistedGrantDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(databaseConfiguration.ConnectionStringsConfiguration.PersistedGrantDbConnection,
                        sql => sql.MigrationsAssembly(
                            databaseConfiguration.DatabaseMigrationsConfiguration?.PersistedGrantDbMigrationsAssembly ??
                            migrationsAssembly)));
        }

        // Add Data Protection DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration
                ?.DataProtectionDbConnection))
        {
            services.AddDbContext<IdentityServerDataProtectionDbContext>(options =>
                options.UseSqlServer(databaseConfiguration.ConnectionStringsConfiguration.DataProtectionDbConnection,
                    sql => sql.MigrationsAssembly(
                        databaseConfiguration.DatabaseMigrationsConfiguration?.DataProtectionDbMigrationsAssembly ??
                        migrationsAssembly)));
        }

        // Add Permission DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.PermissionDbConnection))
        {
            services.AddDbContext<PermissionDbContext>(options =>
                options.UseSqlServer(databaseConfiguration.ConnectionStringsConfiguration.PermissionDbConnection,
                    sql => sql.MigrationsAssembly(
                        databaseConfiguration.DatabaseMigrationsConfiguration?.PermissionDbMigrationsAssembly ??
                        migrationsAssembly)));

            services.AddScoped<IPermissionDbContext>(provider => provider.GetRequiredService<PermissionDbContext>());
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
                    //ServerVersion.AutoDetect(databaseConfiguration.ConnectionStringsConfiguration.IdentityDbConnection) ?? 
                    MySqlServerVersion.LatestSupportedServerVersion,
                    sql => sql.MigrationsAssembly(
                        databaseConfiguration.DatabaseMigrationsConfiguration?.IdentityDbMigrationsAssembly ??
                        migrationsAssembly)));
        }

        // Add Configuration DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.ConfigurationDbConnection))
        {
            services.AddConfigurationDbContext<IdentityServerConfigurationDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseMySql(databaseConfiguration.ConnectionStringsConfiguration.ConfigurationDbConnection,
                        //ServerVersion.AutoDetect(databaseConfiguration.ConnectionStringsConfiguration.ConfigurationDbConnection) ?? 
                        MySqlServerVersion.LatestSupportedServerVersion,
                        sql => sql.MigrationsAssembly(
                            databaseConfiguration.DatabaseMigrationsConfiguration?.ConfigurationDbMigrationsAssembly ??
                            migrationsAssembly)));
        }

        // Add PersistedGrant DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration
                ?.PersistedGrantDbConnection))
        {
            services.AddOperationalDbContext<IdentityServerPersistedGrantDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseMySql(databaseConfiguration.ConnectionStringsConfiguration.PersistedGrantDbConnection,
                        //ServerVersion.AutoDetect(databaseConfiguration.ConnectionStringsConfiguration.PersistedGrantDbConnection) ?? 
                        MySqlServerVersion.LatestSupportedServerVersion,
                        sql => sql.MigrationsAssembly(
                            databaseConfiguration.DatabaseMigrationsConfiguration?.PersistedGrantDbMigrationsAssembly ??
                            migrationsAssembly)));
        }

        // Add Data Protection DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration
                ?.DataProtectionDbConnection))
        {
            services.AddDbContext<IdentityServerDataProtectionDbContext>(options =>
                options.UseMySql(databaseConfiguration.ConnectionStringsConfiguration.DataProtectionDbConnection,
                    //ServerVersion.AutoDetect(databaseConfiguration.ConnectionStringsConfiguration.DataProtectionDbConnection) ?? 
                    MySqlServerVersion.LatestSupportedServerVersion,
                    sql => sql.MigrationsAssembly(
                        databaseConfiguration.DatabaseMigrationsConfiguration?.DataProtectionDbMigrationsAssembly ??
                        migrationsAssembly)));
        }

        // Add Permission DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.PermissionDbConnection))
        {
            services.AddDbContext<PermissionDbContext>(options =>
                options.UseMySql(databaseConfiguration.ConnectionStringsConfiguration.PermissionDbConnection,
                    //ServerVersion.AutoDetect(databaseConfiguration.ConnectionStringsConfiguration.PermissionDbConnection) ?? 
                    MySqlServerVersion.LatestSupportedServerVersion,
                    sql => sql.MigrationsAssembly(
                        databaseConfiguration.DatabaseMigrationsConfiguration?.PermissionDbMigrationsAssembly ??
                        migrationsAssembly)));

            services.AddScoped<IPermissionDbContext>(provider => provider.GetRequiredService<PermissionDbContext>());
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
                    sql => sql.MigrationsAssembly(
                        databaseConfiguration.DatabaseMigrationsConfiguration?.IdentityDbMigrationsAssembly ??
                        migrationsAssembly)));
        }

        // Add Configuration DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.ConfigurationDbConnection))
        {
            services.AddConfigurationDbContext<IdentityServerConfigurationDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseSqlite(databaseConfiguration.ConnectionStringsConfiguration.ConfigurationDbConnection,
                        sql => sql.MigrationsAssembly(
                            databaseConfiguration.DatabaseMigrationsConfiguration?.ConfigurationDbMigrationsAssembly ??
                            migrationsAssembly)));
        }

        // Add PersistedGrant DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration
                ?.PersistedGrantDbConnection))
        {
            services.AddOperationalDbContext<IdentityServerPersistedGrantDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseSqlite(databaseConfiguration.ConnectionStringsConfiguration.PersistedGrantDbConnection,
                        sql => sql.MigrationsAssembly(
                            databaseConfiguration.DatabaseMigrationsConfiguration?.PersistedGrantDbMigrationsAssembly ??
                            migrationsAssembly)));
        }

        // Add Data Protection DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration
                ?.DataProtectionDbConnection))
        {
            services.AddDbContext<IdentityServerDataProtectionDbContext>(options =>
                options.UseSqlite(databaseConfiguration.ConnectionStringsConfiguration.DataProtectionDbConnection,
                    sql => sql.MigrationsAssembly(
                        databaseConfiguration.DatabaseMigrationsConfiguration?.DataProtectionDbMigrationsAssembly ??
                        migrationsAssembly)));
        }

        // Add Permission DbContext
        if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.PermissionDbConnection))
        {
            services.AddDbContext<PermissionDbContext>(options =>
                options.UseSqlite(databaseConfiguration.ConnectionStringsConfiguration.PermissionDbConnection,
                    sql => sql.MigrationsAssembly(
                        databaseConfiguration.DatabaseMigrationsConfiguration?.PermissionDbMigrationsAssembly ??
                        migrationsAssembly)));

            services.AddScoped<IPermissionDbContext>(provider => provider.GetRequiredService<PermissionDbContext>());
        }
    }
}