using Duende.IdentityServer;
using Duende.IdentityServer.Configuration;
using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.Infrastructure.Identity.Services;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Eiromplays.IdentityServer.Infrastructure.Identity;

internal static class Startup
{
    internal static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration, ProjectType projectType)
    {
        if (projectType is ProjectType.Spa) return services;
        
        return services
            .Configure<IdentityOptions>(configuration.GetSection(nameof(IdentityOptions)))
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                configuration.GetSection(nameof(IdentityOptions)).Bind(options))
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .Services
                
            // This is required to redirect the user to the correct page for login
            .ConfigureApplicationCookie(options =>
            {
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = x =>
                    {
                        x.Response.Redirect("https://localhost:3000/auth/login");
                        return Task.CompletedTask;
                    },
                    OnRedirectToLogout = x =>
                    {
                        x.Response.Redirect("https://localhost:3000/auth/logout");
                        return Task.CompletedTask;
                    }
                };

            });
    }

    internal static IServiceCollection AddIdentityServer(this IServiceCollection services, IConfiguration configuration, ProjectType projectType)
    {
        if (projectType is not ProjectType.IdentityServer) return services;

        var identityServerOptions = configuration.GetSection(nameof(IdentityServerOptions));

        return services.AddIdentityServer(options => identityServerOptions.Bind(options))
            .AddConfigurationStore<ApplicationDbContext>()
            .AddOperationalStore<ApplicationDbContext>(options =>
            {
                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 3600; // interval in seconds (default is 3600)
            })
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<CustomProfileService>()
            .AddServerSideSessions()
            .Services
            .AddOpenTelemetryTracing(projectType);
    }

    internal static IServiceCollection AddOpenTelemetryTracing(this IServiceCollection services,
        ProjectType projectType)
    {
        if (projectType != ProjectType.Spa) return services;

        return services
            .AddOpenTelemetryTracing(builder =>
            {
                builder
                    // all available sources
                    .AddSource(IdentityServerConstants.Tracing.Basic)
                    .AddSource(IdentityServerConstants.Tracing.Cache)
                    .AddSource(IdentityServerConstants.Tracing.Services)
                    .AddSource(IdentityServerConstants.Tracing.Stores)
                    .AddSource(IdentityServerConstants.Tracing.Validation)
                
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService("IdentityServer"))
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddSqlClientInstrumentation()
                    
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = "localhost";
                        options.AgentPort = 6831;
                        options.ExportProcessorType = ExportProcessorType.Simple;
                    })
                    .AddConsoleExporter(options =>
                    {
                        options.Targets = ConsoleExporterOutputTargets.Console;
                    });
            });
    }

    internal static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration, ProjectType projectType)
    {
        if (projectType is not ProjectType.IdentityServer) return services;
        
        var authenticationBuilder = services.AddAuthentication();

        // TODO: Find a better way to register external providers
        var externalProviderConfiguration = configuration.GetSection(nameof(ExternalProvidersConfiguration))
            .Get<ExternalProvidersConfiguration>();
        
        if (externalProviderConfiguration.UseGoogleProvider &&
            !string.IsNullOrWhiteSpace(externalProviderConfiguration.GoogleClientId) &&
            !string.IsNullOrWhiteSpace(externalProviderConfiguration.GoogleClientSecret))
        {
            authenticationBuilder.AddGoogle(options =>
            {
                options.ClientId = externalProviderConfiguration.GoogleClientId;
                options.ClientSecret = externalProviderConfiguration.GoogleClientSecret;

                if (!string.IsNullOrEmpty(externalProviderConfiguration.GoogleCallbackPath))
                    options.CallbackPath = externalProviderConfiguration.GoogleCallbackPath;
            });
        }

        if (externalProviderConfiguration.UseGitHubProvider &&
            !string.IsNullOrWhiteSpace(externalProviderConfiguration.GitHubClientId) &&
            !string.IsNullOrWhiteSpace(externalProviderConfiguration.GitHubClientSecret))
        {
            authenticationBuilder.AddGitHub(options =>
            {
                options.ClientId = externalProviderConfiguration.GitHubClientId;
                options.ClientSecret = externalProviderConfiguration.GitHubClientSecret;
                if (!string.IsNullOrEmpty(externalProviderConfiguration.GitHubCallbackPath))
                    options.CallbackPath = externalProviderConfiguration.GitHubCallbackPath;

                options.Scope.Add("user:email");
            });
        }

        if (externalProviderConfiguration.UseDiscordProvider &&
            !string.IsNullOrWhiteSpace(externalProviderConfiguration.DiscordClientId) &&
            !string.IsNullOrWhiteSpace(externalProviderConfiguration.DiscordClientSecret))
        {
            authenticationBuilder.AddDiscord(options =>
            {
                options.ClientId = externalProviderConfiguration.DiscordClientId;
                options.ClientSecret = externalProviderConfiguration.DiscordClientSecret;

                if (!string.IsNullOrWhiteSpace(externalProviderConfiguration.DiscordCallbackPath))
                    options.CallbackPath = externalProviderConfiguration.DiscordCallbackPath;

                options.Scope.Add("email");
                options.SaveTokens = true;
            });
        }

        if (externalProviderConfiguration.UseRedditProvider &&
            !string.IsNullOrWhiteSpace(externalProviderConfiguration.RedditClientId) &&
            !string.IsNullOrWhiteSpace(externalProviderConfiguration.RedditClientSecret))
        {
            authenticationBuilder.AddReddit(options =>
            {
                options.ClientId = externalProviderConfiguration.RedditClientId;
                options.ClientSecret = externalProviderConfiguration.RedditClientSecret;

                if (!string.IsNullOrEmpty(externalProviderConfiguration.RedditCallbackPath))
                    options.CallbackPath = externalProviderConfiguration.RedditCallbackPath;
            });
        }

        if (externalProviderConfiguration.UseAmazonProvider &&
            !string.IsNullOrWhiteSpace(externalProviderConfiguration.AmazonClientId) &&
            !string.IsNullOrWhiteSpace(externalProviderConfiguration.AmazonClientSecret))
        {
            authenticationBuilder.AddAmazon(options =>
            {
                options.ClientId = externalProviderConfiguration.AmazonClientId;
                options.ClientSecret = externalProviderConfiguration.AmazonClientSecret;

                if (!string.IsNullOrEmpty(externalProviderConfiguration.AmazonCallbackPath))
                    options.CallbackPath = externalProviderConfiguration.AmazonCallbackPath;
            });
        }

        if (externalProviderConfiguration.UseTwitchProvider &&
            !string.IsNullOrWhiteSpace(externalProviderConfiguration.TwitchClientId) &&
            !string.IsNullOrWhiteSpace(externalProviderConfiguration.TwitchClientSecret))
        {
            authenticationBuilder.AddTwitch(options =>
            {
                options.ClientId = externalProviderConfiguration.TwitchClientId;
                options.ClientSecret = externalProviderConfiguration.TwitchClientSecret;

                if (!string.IsNullOrEmpty(externalProviderConfiguration.TwitchCallbackPath))
                    options.CallbackPath = externalProviderConfiguration.TwitchCallbackPath;
            });
        }

        if (externalProviderConfiguration.UsePatreonProvider &&
            !string.IsNullOrWhiteSpace(externalProviderConfiguration.PatreonClientId) &&
            !string.IsNullOrWhiteSpace(externalProviderConfiguration.PatreonClientSecret))
        {
            authenticationBuilder.AddPatreon(options =>
            {
                options.ClientId = externalProviderConfiguration.PatreonClientId;
                options.ClientSecret = externalProviderConfiguration.PatreonClientSecret;
                options.Scope.Add("identity[email]");

                if (!string.IsNullOrEmpty(externalProviderConfiguration.PatreonCallbackPath))
                    options.CallbackPath = externalProviderConfiguration.PatreonCallbackPath;
            });
        }

        if (externalProviderConfiguration.UseWordpressProvider &&
            !string.IsNullOrWhiteSpace(externalProviderConfiguration.WordpressClientId) &&
            !string.IsNullOrWhiteSpace(externalProviderConfiguration.WordpressClientSecret))
        {
            authenticationBuilder.AddWordPress(options =>
            {
                options.ClientId = externalProviderConfiguration.WordpressClientId;
                options.ClientSecret = externalProviderConfiguration.WordpressClientSecret;

                if (!string.IsNullOrEmpty(externalProviderConfiguration.WordpressCallbackPath))
                    options.CallbackPath = externalProviderConfiguration.WordpressCallbackPath;
            });
        }

        return authenticationBuilder.Services;
    }
}