using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.EntityFramework.Storage;
using IdentityForge.IdentityServer.Configurations;
using IdentityForge.IdentityServer.Database;
using IdentityForge.IdentityServer.Domain.Roles;
using IdentityForge.IdentityServer.Domain.Users;
using IdentityForge.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityForge.IdentityServer.Extensions.Services;

public static class ServiceRegistration
{
    public static void AddInfrastructure(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
    {
        string connStr = GetDatabaseConnectionString(env, configuration.GetConnectionStringOptions().IdentityForge);
        AddDbContext(services, connStr);
        services.AddHostedService<MigrationHostedService<IdentityForgeDbContext>>();
        AddAuth(services, env, configuration);
        ConfigureIdentity(services, configuration, configuration.GetIdentityServerOptions());
        ConfigureIdentityServer(services, configuration);
    }

    private static string GetDatabaseConnectionString(IHostEnvironment env, string configuredConnectionString)
    {
        if(!string.IsNullOrWhiteSpace(configuredConnectionString))
            return configuredConnectionString;

        if(!env.IsDevelopment())
            throw new Exception("The database connection string is not set.");

        return "Host=localhost;Port=36327;Database=dev_identityforge;Username=postgres;Password=postgres";
    }

    private static void AddDbContext(IServiceCollection services, string connectionString)
    {
        services.AddDbContext<IdentityForgeDbContext>(options =>
                options.UseNpgsql(connectionString,
                        builder => builder.MigrationsAssembly(typeof(IdentityForgeDbContext).Assembly.FullName))
                    .UseSnakeCaseNamingConvention())
            .AddConfigurationDbContext<IdentityForgeDbContext>()
            .AddOperationalDbContext<IdentityForgeDbContext>();
    }

    private static void AddAuth(IServiceCollection services, IHostEnvironment env, IConfiguration configuration)
    {
        if (env.IsEnvironment(Consts.Testing.FunctionalTestingEnvName))
            return;

        var authOptions = configuration.GetAuthOptions();
        var authenticationBuilder = services.AddAuthentication();

        foreach (var externalProvider in authOptions.ExternalProviders.Where(provider =>
                     provider.IsEnabled && !string.IsNullOrWhiteSpace(provider.ClientId) &&
                     !string.IsNullOrWhiteSpace(provider.ClientSecret)))
        {
            AddExternalProvider(authenticationBuilder, externalProvider);
        }
    }

    #region External Providers

    private static AuthenticationBuilder AddExternalProvider(AuthenticationBuilder authenticationBuilder, ExternalProvider provider) =>
        provider.Name.ToLower() switch
        {
            "google" => AddGoogleProvider(authenticationBuilder, provider),
            "github" => AddGitHubProvider(authenticationBuilder, provider),
            "discord" => AddDiscordProvider(authenticationBuilder, provider),
            _ => throw new Exception($"Unknown external provider: {provider.Name}")
        };

    private static AuthenticationBuilder AddGoogleProvider(AuthenticationBuilder builder, ExternalProvider provider) =>
        builder.AddGoogle(options =>
        {
            options.ClientId = provider.ClientId;
            options.ClientSecret = provider.ClientSecret;

            if (!string.IsNullOrEmpty(provider.CallbackPath))
                options.CallbackPath = provider.CallbackPath;
        });

    private static AuthenticationBuilder AddGitHubProvider(AuthenticationBuilder builder, ExternalProvider provider) =>
        builder.AddGitHub(options =>
        {
            options.ClientId = provider.ClientId;
            options.ClientSecret = provider.ClientSecret;

            if (!string.IsNullOrEmpty(provider.CallbackPath))
                options.CallbackPath = provider.CallbackPath;

            options.Scope.Add("user:email");
        });

    private static AuthenticationBuilder AddDiscordProvider(AuthenticationBuilder builder, ExternalProvider provider) =>
        builder.AddDiscord(options =>
        {
            options.ClientId = provider.ClientId;
            options.ClientSecret = provider.ClientSecret;

            if (!string.IsNullOrWhiteSpace(provider.CallbackPath))
                options.CallbackPath = provider.CallbackPath;

            options.Scope.Add("email");
            options.SaveTokens = true;
        });

    #endregion

    private static void ConfigureIdentity(IServiceCollection services, IConfiguration configuration, IdentityServerOptions identityServerOptions)
    {
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                configuration.GetIdentityOptionsSection().Bind(options))
            .AddEntityFrameworkStores<IdentityForgeDbContext>()
            .AddDefaultTokenProviders()
            .Services
            .ConfigureApplicationCookie(options =>
            {
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = x =>
                        x.Response.SendRedirectAsync(
                            identityServerOptions.UserInteraction.LoginUrl ??
                            throw new InvalidOperationException("LoginUrl is not set in IdentityServerOptions"), true),
                    OnRedirectToLogout = x =>
                        x.Response.SendRedirectAsync(
                            identityServerOptions.UserInteraction.LogoutUrl ??
                            throw new InvalidOperationException("LogoutUrl is not set in IdentityServerOptions"), true)
                };
            });
    }

    private static void ConfigureIdentityServer(IServiceCollection services, IConfiguration configuration)
    {
        var identityServerOptions = configuration.GetSection(nameof(IdentityServerOptions));

        services.AddIdentityServer(options => identityServerOptions.Bind(options))
            .AddConfigurationStore<IdentityForgeDbContext>()
            .AddOperationalStore<IdentityForgeDbContext>(options =>
            {
                // Enable automatic token cleanup. (This is optional)
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 3600; // Interval in seconds (default is 3600/1 hour)
            })
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<IdentityForgeProfileService>()
            .AddServerSideSessions();
    }
}
