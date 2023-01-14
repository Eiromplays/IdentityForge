using System.Reflection;
using System.Runtime.CompilerServices;
using Duende.Bff;
using Duende.Bff.Yarp;
using Eiromplays.IdentityServer.Application.Common.Caching;
using Eiromplays.IdentityServer.Application.Common.Configurations;
using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Application.Common.Configurations.Identity;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure.Auth;
using Eiromplays.IdentityServer.Infrastructure.BackgroundJobs;
using Eiromplays.IdentityServer.Infrastructure.Caching;
using Eiromplays.IdentityServer.Infrastructure.Common;
using Eiromplays.IdentityServer.Infrastructure.Cors;
using Eiromplays.IdentityServer.Infrastructure.FileStorage;
using Eiromplays.IdentityServer.Infrastructure.Localization;
using Eiromplays.IdentityServer.Infrastructure.Mailing;
using Eiromplays.IdentityServer.Infrastructure.Mapping;
using Eiromplays.IdentityServer.Infrastructure.Middleware;
using Eiromplays.IdentityServer.Infrastructure.Notifications;
using Eiromplays.IdentityServer.Infrastructure.OpenApi;
using Eiromplays.IdentityServer.Infrastructure.Persistence;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Initialization;
using Eiromplays.IdentityServer.Infrastructure.SecurityHeaders;
using Eiromplays.IdentityServer.Infrastructure.Sms;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Azure.Identity;
using Eiromplays.IdentityServer.Application.Common;
using Eiromplays.IdentityServer.Infrastructure.HttpClients;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.DataProtection;

[assembly: InternalsVisibleTo("Infrastructure.Test")]

namespace Eiromplays.IdentityServer.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager config, ProjectType projectType)
    {
        if (projectType is ProjectType.Spa) return services.AddInfrastructureSpa(config, projectType);

        MapsterSettings.Configure();
        return services
            .AddConfigurations(config)
            .AddBackgroundJobs(config)
            .AddCaching(config)
            .AddCorsPolicy(config)
            .AddExceptionMiddleware()
            .AddHealthCheck()
            .AddPoLocalization(config)
            .AddMailing(config)
            .AddSms(config)
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddNotifications(config)
            .AddOpenApiDocumentation(config)
            .AddPersistence(config, projectType)
            .AddDataProtection()
            .PersistKeysToDbContext<ApplicationDbContext>()
            .Services
            .AddAuth(config, projectType)
            .AddRequestLogging(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddServices(projectType)
            .AddHttpClients(config, projectType)
            .AddCloudflareImagesStorageService(config);
    }

    private static IServiceCollection AddInfrastructureSpa(this IServiceCollection services, ConfigurationManager config, ProjectType projectType)
    {
        if (projectType is not ProjectType.Spa) return services;

        config.AddExternalSecretProviders();

        var bffBuilder = services.AddBff(options => config.GetSection(nameof(BffOptions)).Bind(options));
        bffBuilder.AddRemoteApis();
        bffBuilder.AddBffPersistence(config, projectType);

        services.AddReverseProxy()
            .AddBffExtensions()
            .LoadFromConfig(config.GetSection("ReverseProxy"));

        return services;
    }

    private static IServiceCollection AddHealthCheck(this IServiceCollection services) =>
        services.AddHealthChecks().Services;

    public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        // Create a new scope to retrieve scoped services
        using var scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
            .InitializeDatabasesAsync(cancellationToken).ConfigureAwait(false);
    }

    public static WebApplication UseInfrastructure(this WebApplication app, IConfiguration config, ProjectType projectType, Action<Config>? fastEndpointsConfigAction = null)
    {
        if (projectType is ProjectType.Spa) return app;

        app
            .UseRequestLocalization()
            .UseStaticFiles()
            .UseSecurityHeaders(config)
            .UseFileStorage()
            .UseExceptionMiddleware()
            .UseRouting()
            .UseCorsPolicy()
            .UseAuthentication()
            .UseIdentityServer(projectType)
            .UseAuthorization()
            .UseCurrentUser()
            .UseRequestLogging(config)
            .UseHangfireDashboard(config);

        app.UseFastEndpoints(fastEndpointsConfigAction);

        app.UseOpenApiDocumentation(config);

        return app;
    }

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers().RequireAuthorization();
        builder.MapHealthCheck();
        builder.MapNotifications();
        return builder;
    }

    // ReSharper disable once UnusedMethodReturnValue.Local
    private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapHealthChecks("/api/health").RequireAuthorization();

    private static ConfigurationManager AddExternalSecretProviders(this ConfigurationManager configuration)
    {
        if (!configuration.GetSection(nameof(SecretsConfiguration)).Exists()) return configuration;

        var secretsConfiguration = configuration.GetSection(nameof(SecretsConfiguration)).Get<SecretsConfiguration>();

        if (secretsConfiguration is null) return configuration;
        configuration.AddAwsSecretsManager(secretsConfiguration.AwsSecretsManagerConfiguration);

        configuration.AddAzureKeyVaultConfiguration(secretsConfiguration.AzureKeyVaultConfiguration);

        return configuration;
    }

    // Registers Azure Key Vault as a source for configuration values.
    private static ConfigurationManager AddAzureKeyVaultConfiguration(this ConfigurationManager configuration, AzureKeyVaultConfiguration? azureKeyVaultConfiguration)
    {
        try
        {
            if (azureKeyVaultConfiguration is null or { Enabled: false}) return configuration;

            configuration.AddAzureKeyVault(new Uri(azureKeyVaultConfiguration.KeyVaultUrl), new DefaultAzureCredential(), new CustomKeyVaultSecretManager());
        }
        catch (Exception e)
        {
            Console.WriteLine($"Azure Key Vault configuration failed. {e.Message}");
        }

        return configuration;
    }

    // Registers AWS Secrets Manager as a source for configuration values.
    private static ConfigurationManager AddAwsSecretsManager(this ConfigurationManager configuration, AwsSecretsManagerConfiguration? awsSecretsManagerConfiguration)
    {
        if (awsSecretsManagerConfiguration is null or { Enabled: false}) return configuration;

        try
        {
            configuration.AddSecretsManager(
                configurator: config =>
                {
                    config.KeyGenerator = (_, name) =>
                    {
                        string prefix = awsSecretsManagerConfiguration.AllowedPrefixes.First(name.StartsWith);

                        name = name.Replace(prefix, string.Empty, StringComparison.OrdinalIgnoreCase).Replace("__", ":", StringComparison.OrdinalIgnoreCase);
                        if (name.StartsWith(":", StringComparison.OrdinalIgnoreCase))
                            name = name[1..];

                        return name;
                    };

                    config.SecretFilter = secret => awsSecretsManagerConfiguration.AllowedPrefixes.Any(allowed => secret.Name.StartsWith(allowed, StringComparison.OrdinalIgnoreCase));
                });
        }
        catch (Exception e)
        {
            Console.WriteLine($"AWS Secrets Manager configuration failed. {e.Message}");
        }

        return configuration;
    }

    /*
        Configures custom classes for config files, so they can be retrieved from DI using IOptions<T>
        Information:
        Account Configuration:
        Profile Picture Configuration:
        Find more avatar styles here: https://avatars.dicebear.com/styles/
        You can also use a custom provider
    */
    private static IServiceCollection AddConfigurations(this IServiceCollection services, ConfigurationManager configuration)
    {
        configuration.AddExternalSecretProviders();

        return services.Configure<AccountConfiguration>(configuration.GetSection(nameof(AccountConfiguration)))
            .Configure<IdentityServerData>(configuration.GetSection(nameof(IdentityServerData)))
            .Configure<IdentityData>(configuration.GetSection(nameof(IdentityData)))
            .Configure<CloudflareConfiguration>(configuration.GetSection(nameof(CloudflareConfiguration)))
            .Configure<SecretsConfiguration>(configuration.GetSection(nameof(SecretsConfiguration)));
    }

    private static IApplicationBuilder UseIdentityServer(this IApplicationBuilder builder, ProjectType projectType)
    {
        return projectType is ProjectType.IdentityServer ? builder.UseIdentityServer() : builder;
    }
}