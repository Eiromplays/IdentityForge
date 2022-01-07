using Duende.IdentityServer;
using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.EntityFramework.Storage;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Identity.Common.Interfaces;
using Eiromplays.IdentityServer.Infrastructure.Identity.Configurations;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.Infrastructure.Identity.Persistence.DbContexts;
using Eiromplays.IdentityServer.Infrastructure.Identity.Services;
using Eiromplays.IdentityServer.Infrastructure.Persistence.DbContexts;
using Eiromplays.IdentityServer.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace Eiromplays.IdentityServer.Infrastructure.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConfiguration =
            configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();

        services.RegisterNpgSqlDbContexts(databaseConfiguration);

        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddAuthentication(configuration);

        services.AddIdentityServer(configuration);

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        return services;
    }

    public static void RegisterNpgSqlDbContexts(this IServiceCollection services, DatabaseConfiguration databaseConfiguration)
    {
        var migrationsAssembly = typeof(DependencyInjection).GetTypeInfo().Assembly.GetName().Name;

        // Add Identity DbContext
        if (databaseConfiguration.UseInMemoryDatabase)
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseInMemoryDatabase("EiromplaysIdentityServerDb"));
        }
        else if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.IdentityDbConnection))
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseNpgsql(databaseConfiguration.ConnectionStringsConfiguration.IdentityDbConnection,
                    sql => sql.MigrationsAssembly(
                        databaseConfiguration.DatabaseMigrationsConfiguration?.IdentityDbMigrationsAssembly ??
                        migrationsAssembly)));
        }

        // Add Configuration DbContext
        if (databaseConfiguration.UseInMemoryDatabase)
        {
            services.AddConfigurationDbContext<IdentityServerConfigurationDbContext>(options => options.ConfigureDbContext = sql =>
                sql.UseInMemoryDatabase("EiromplaysIdentityServerConfigurationDb"));
        }
        else if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.ConfigurationDbConnection))
        {
            services.AddConfigurationDbContext<IdentityServerConfigurationDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseNpgsql(databaseConfiguration.ConnectionStringsConfiguration.ConfigurationDbConnection,
                        sql => sql.MigrationsAssembly(
                            databaseConfiguration.DatabaseMigrationsConfiguration?.ConfigurationDbMigrationsAssembly ??
                            migrationsAssembly)));
        }

        // Add PersistedGrant DbContext
        if (databaseConfiguration.UseInMemoryDatabase)
        {
            services.AddOperationalDbContext<IdentityServerPersistedGrantDbContext>(options => options.ConfigureDbContext = sql =>
                sql.UseInMemoryDatabase("EiromplaysIdentityServerPersistedGrantDb"));
        }
        else if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.PersistedGrantDbConnection))
        {
            services.AddOperationalDbContext<IdentityServerPersistedGrantDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseNpgsql(databaseConfiguration.ConnectionStringsConfiguration.PersistedGrantDbConnection,
                        sql => sql.MigrationsAssembly(
                            databaseConfiguration.DatabaseMigrationsConfiguration?.PersistedGrantDbMigrationsAssembly ??
                            migrationsAssembly)));
        }

        // Add Data Protection DbContext
        if (databaseConfiguration.UseInMemoryDatabase)
        {
            services.AddDbContext<IdentityServerDataProtectionDbContext>(options =>
                options.UseInMemoryDatabase("EiromplaysIdentityServerDataProtectionDb"));
        }
        else if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.DataProtectionDbConnection))
        {
            services.AddDbContext<IdentityServerDataProtectionDbContext>(options =>
                options.UseNpgsql(databaseConfiguration.ConnectionStringsConfiguration.DataProtectionDbConnection,
                    sql => sql.MigrationsAssembly(
                        databaseConfiguration.DatabaseMigrationsConfiguration?.DataProtectionDbMigrationsAssembly ??
                        migrationsAssembly)));
        }

        // Add Permission DbContext
        if (databaseConfiguration.UseInMemoryDatabase)
        {
            services.AddDbContext<PermissionDbContext>(options =>
                options.UseInMemoryDatabase("EiromplaysPermissionDb"));
        }
        else if (!string.IsNullOrWhiteSpace(databaseConfiguration.ConnectionStringsConfiguration?.PermissionDbConnection))
        {
            services.AddDbContext<PermissionDbContext>(options =>
                options.UseNpgsql(databaseConfiguration.ConnectionStringsConfiguration.PermissionDbConnection,
                    sql => sql.MigrationsAssembly(
                        databaseConfiguration.DatabaseMigrationsConfiguration?.PermissionDbMigrationsAssembly ??
                        migrationsAssembly)));

            services.AddScoped<IPermissionDbContext>(provider => provider.GetRequiredService<PermissionDbContext>());
        }
    }

    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var identityOptions = configuration.GetSection(nameof(IdentityOptions)).Get<IdentityOptions>();

        services
            .AddSingleton(identityOptions)
            .AddIdentity<ApplicationUser, ApplicationRole>(options => configuration.GetSection(nameof(IdentityOptions)).Bind(options))
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication()
            .AddOpenIdConnect("oidc", "Demo IdentityServer", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                options.SaveTokens = true;

                options.Authority = "https://demo.duendesoftware.com/";
                options.ClientId = "interactive.confidential";
                options.ClientSecret = "secret";
                options.ResponseType = "code";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });
    }

    public static void AddIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationSection = configuration.GetSection(nameof(IdentityServerOptions));

        services.AddIdentityServer(options => configurationSection.Bind(options))
            .AddConfigurationStore<IdentityServerConfigurationDbContext>()
            .AddOperationalStore<IdentityServerPersistedGrantDbContext>()
            .AddAspNetIdentity<ApplicationUser>();
    }

    public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var databaseConfiguration =
            configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();

        if (databaseConfiguration.DatabaseMigrationsConfiguration is not null &&
            !databaseConfiguration.DatabaseMigrationsConfiguration.ApplyDatabaseMigrations) return;

        await using var scope = serviceProvider.CreateAsyncScope();

        var services = scope.ServiceProvider;

        try
        {
            await using var identityDbContext = services.GetRequiredService<IdentityDbContext>();

            if (identityDbContext.Database.IsNpgsql())
            {
                await identityDbContext.Database.MigrateAsync();
            }

            await using var identityServerConfigurationDbContext = services.GetRequiredService<IdentityServerConfigurationDbContext>();

            if (identityServerConfigurationDbContext.Database.IsNpgsql())
            {
                await identityServerConfigurationDbContext.Database.MigrateAsync();
            }

            await using var identityServerPersistedGrantDbContext = services.GetRequiredService<IdentityServerPersistedGrantDbContext>();

            if (identityServerPersistedGrantDbContext.Database.IsNpgsql())
            {
                await identityServerPersistedGrantDbContext.Database.MigrateAsync();
            }

            await using var identityServerDataProtectionDbContext = services.GetRequiredService<IdentityServerDataProtectionDbContext>();

            if (identityServerDataProtectionDbContext.Database.IsNpgsql())
            {
                await identityServerDataProtectionDbContext.Database.MigrateAsync();
            }

            await using var permissionDbContext = services.GetRequiredService<PermissionDbContext>();

            if (permissionDbContext.Database.IsNpgsql())
            {
                await permissionDbContext.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

            logger.LogError(ex, "An error occurred while migrating or seeding the database(s).");

            throw;
        }
    }
}