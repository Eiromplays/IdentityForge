using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.EntityFramework.Storage;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Identity.Common.Interfaces;
using Eiromplays.IdentityServer.Domain.Constants;
using Eiromplays.IdentityServer.Infrastructure.Helpers;
using Eiromplays.IdentityServer.Infrastructure.Identity.Configurations;
using Eiromplays.IdentityServer.Infrastructure.Identity.Configurations.Identity;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.Infrastructure.Identity.Persistence.DbContexts;
using Eiromplays.IdentityServer.Infrastructure.Identity.Persistence.DbContexts.Seeds;
using Eiromplays.IdentityServer.Infrastructure.Identity.Services;
using Eiromplays.IdentityServer.Infrastructure.Persistence.DbContexts;
using Eiromplays.IdentityServer.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using Eiromplays.IdentityServer.Domain.Enums;
using FluentEmail.Graph;
using FluentEmail.Mailgun;
using FluentEmail.MailKitSmtp;

namespace Eiromplays.IdentityServer.Infrastructure.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConfiguration =
            configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();

        RegisterIdentityDataConfiguration(services, configuration);

        services.RegisterNpgSqlDbContexts(databaseConfiguration);

        services.AddDataProtection();

        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddAuthentication(configuration);

        services.AddIdentityServer(configuration);

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        services.AddEmailSenders(configuration);

        return services;
    }

    private static void RegisterIdentityDataConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        var identityDataConfiguration = configuration.GetSection(nameof(IdentityData)).Get<IdentityData>();

        services.AddSingleton(identityDataConfiguration);
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
            .AddIdentity<ApplicationUser, ApplicationRole>(options => configuration.GetSection(nameof(IdentityOptions)).Bind(options))
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddExternalProviders(configuration);
    }

    public static void AddIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationSection = configuration.GetSection(nameof(IdentityServerOptions));

        services.AddIdentityServer(options => configurationSection.Bind(options))
            .AddConfigurationStore<IdentityServerConfigurationDbContext>()
            .AddOperationalStore<IdentityServerPersistedGrantDbContext>()
            .AddAspNetIdentity<ApplicationUser>();
    }

    public static void AddExternalProviders(this IServiceCollection services, IConfiguration configuration)
    {
        var externalProviderConfiguration = configuration.GetSection(nameof(ExternalProvidersConfiguration))
            .Get<ExternalProvidersConfiguration>();

        var authenticationBuilder = services.AddAuthentication();

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

    public static void AddEmailSenders(this IServiceCollection services, IConfiguration configuration)
    {
        var emailConfiguration = configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>();
        var fluentEmailServicesBuilder =services
            .AddFluentEmail(emailConfiguration.From, emailConfiguration.DefaultFromName)
            .AddRazorRenderer();

        Console.WriteLine($"EmailProvider {emailConfiguration.EmailProvider}");
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
                throw new ArgumentOutOfRangeException(nameof(emailConfiguration.EmailProvider), $"EmailProvider needs to be one of these: {string.Join(", ", Enum.GetNames(typeof(EmailProvider)))}.");
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

    public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var databaseConfiguration =
            configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();

        if (databaseConfiguration.DatabaseMigrationsConfiguration is not null &&
            !databaseConfiguration.DatabaseMigrationsConfiguration.ApplyDatabaseMigrations)
            return;

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
    }
}