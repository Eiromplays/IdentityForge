using System.Net;
using System.Net.Mail;
using Duende.IdentityServer.Configuration;
using Eiromplays.IdentityServer.Application.Common.Configurations;
using Eiromplays.IdentityServer.Application.Common.Configurations.Database;
using Eiromplays.IdentityServer.Application.Common.Configurations.Identity;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Domain.Constants;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure.Helpers;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.Infrastructure.Persistence.DbContexts;
using Eiromplays.IdentityServer.Infrastructure.Persistence.DbContexts.Seeds;
using Eiromplays.IdentityServer.Infrastructure.Services;
using FluentEmail.Graph;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, ProjectType projectType)
    {
        var databaseConfiguration =
            configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();

        services.RegisterIdentityDataConfiguration(configuration, projectType);

        services.RegisterDbContexts(databaseConfiguration);

        services.AddDataProtection();

        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddAuthentication(configuration, projectType);

        services.AddIdentityServer(configuration, projectType);

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        services.AddEmailSenders(configuration);

        return services;
    }

    private static void RegisterIdentityDataConfiguration(this IServiceCollection services, IConfiguration configuration, ProjectType projectType)
    {
        if (projectType is not ProjectType.IdentityServer) return;

        var identityDataConfiguration = configuration.GetSection(nameof(IdentityData)).Get<IdentityData>();

        services.AddSingleton(identityDataConfiguration);

        var identityServerDataConfiguration = configuration.GetSection(nameof(IdentityServerData)).Get<IdentityServerData>();

        services.AddSingleton(identityServerDataConfiguration);
    }

    private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration, ProjectType projectType)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            options.Secure = CookieSecurePolicy.SameAsRequest;
            options.OnAppendCookie = cookieContext =>
                AuthenticationHelpers.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            options.OnDeleteCookie = cookieContext =>
                AuthenticationHelpers.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
        });

        var identityOptions = configuration.GetSection(nameof(IdentityOptions)).Get<IdentityOptions>();

        services
            .AddSingleton(identityOptions)
            .AddScoped<IUserResolver<ApplicationUser>, UserResolver>()
            .AddIdentity<ApplicationUser, ApplicationRole>(options => configuration.GetSection(nameof(IdentityOptions)).Bind(options))
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddExternalProviders(configuration, projectType);
    }

    private static void AddIdentityServer(this IServiceCollection services, IConfiguration configuration, ProjectType projectType)
    {
        if (projectType is not ProjectType.IdentityServer) return;

        var configurationSection = configuration.GetSection(nameof(IdentityServerOptions));

        services.AddIdentityServer(options => configurationSection.Bind(options))
            .AddConfigurationStore<IdentityServerConfigurationDbContext>()
            .AddOperationalStore<IdentityServerPersistedGrantDbContext>()
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<CustomProfileService>();
    }

    private static void AddExternalProviders(this IServiceCollection services, IConfiguration configuration, ProjectType projectType)
    {
        if (projectType is not ProjectType.IdentityServer) return;

        var authenticationBuilder = services.AddAuthentication();

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
    }

    private static void AddEmailSenders(this IServiceCollection services, IConfiguration configuration)
    {
        var emailConfiguration = configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>();
        var fluentEmailServicesBuilder =services
            .AddFluentEmail(emailConfiguration.From, emailConfiguration.DefaultFromName)
            .AddRazorRenderer();

        switch (emailConfiguration.EmailProvider)
        {
            case EmailProvider.Smtp:
                if (emailConfiguration.SmtpEmailConfiguration is null)
                    break;
                fluentEmailServicesBuilder.AddSmtpSender(new SmtpClient(emailConfiguration.SmtpEmailConfiguration.Host, emailConfiguration.SmtpEmailConfiguration.Port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailConfiguration.SmtpEmailConfiguration.Login, emailConfiguration.SmtpEmailConfiguration.Password),
                    EnableSsl = emailConfiguration.SmtpEmailConfiguration.UseSsl
                });
                break;
            case EmailProvider.MailKit:
                if (emailConfiguration.MailKitConfiguration is null)
                    break;
                fluentEmailServicesBuilder.AddMailKitSender(emailConfiguration.MailKitConfiguration);
                break;
            case EmailProvider.SendGrid:
                if (emailConfiguration.SendGridConfiguration is null)
                    break;
                fluentEmailServicesBuilder.AddSendGridSender(emailConfiguration.SendGridConfiguration.ApiKey, emailConfiguration.SendGridConfiguration.SandboxMode);
                break;
            case EmailProvider.Mailgun:
                if (emailConfiguration.MailgunConfiguration is null)
                    break;
                fluentEmailServicesBuilder.AddMailGunSender(emailConfiguration.MailgunConfiguration.DomainName,
                    emailConfiguration.MailgunConfiguration.ApiKey, emailConfiguration.MailgunConfiguration.Region);
                break;
            case EmailProvider.Mailtrap:
                if (emailConfiguration.MailtrapConfiguration is null)
                    break;
                fluentEmailServicesBuilder.AddMailtrapSender(emailConfiguration.MailtrapConfiguration.UserName,
                    emailConfiguration.MailtrapConfiguration.Password, emailConfiguration.MailtrapConfiguration.Host,
                    emailConfiguration.MailtrapConfiguration.Port);
                break;
            case EmailProvider.Graph:
                if (emailConfiguration.GraphConfiguration is null)
                    break;
                fluentEmailServicesBuilder.AddGraphSender(emailConfiguration.GraphConfiguration);
                break;
            default:
                var nameOfEmailProvider = nameof(emailConfiguration.EmailProvider);
                throw new ArgumentOutOfRangeException(nameOfEmailProvider, $"EmailProvider needs to be one of these: {string.Join(", ", Enum.GetNames(typeof(EmailProvider)))}.");
        }
    }

    public static void UseSecurityHeaders(this IApplicationBuilder app, IConfiguration configuration)
    {
        var forwardingOptions = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        };

        forwardingOptions.KnownNetworks.Clear();
        forwardingOptions.KnownProxies.Clear();

        app.UseForwardedHeaders(forwardingOptions);

        app.UseReferrerPolicy(options => options.NoReferrer());

        // CSP Configuration to be able to use external resources
        var cspTrustedDomains = new List<string>();
        configuration.GetSection(ConfigurationConsts.CspTrustedDomainsKey).Bind(cspTrustedDomains);
        if (cspTrustedDomains.Any())
        {
            app.UseCsp(csp =>
            {
                csp.ImageSources(options =>
                {
                    options.SelfSrc = true;
                    options.CustomSources = cspTrustedDomains;
                    options.Enabled = true;
                });
                csp.FontSources(options =>
                {
                    options.SelfSrc = true;
                    options.CustomSources = cspTrustedDomains;
                    options.Enabled = true;
                });
                csp.ScriptSources(options =>
                {
                    options.SelfSrc = true;
                    options.CustomSources = cspTrustedDomains;
                    options.Enabled = true;
                    options.UnsafeInlineSrc = true;
                });
                csp.StyleSources(options =>
                {
                    options.SelfSrc = true;
                    options.CustomSources = cspTrustedDomains;
                    options.Enabled = true;
                    options.UnsafeInlineSrc = true;
                });
                csp.DefaultSources(options =>
                {
                    options.SelfSrc = true;
                    options.CustomSources = cspTrustedDomains;
                    options.Enabled = true;
                });
            });
        }
    }
}