using System.Reflection;
using System.Runtime.CompilerServices;
using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
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
using Eiromplays.IdentityServer.Infrastructure.Multitenancy;
using Eiromplays.IdentityServer.Infrastructure.Notifications;
using Eiromplays.IdentityServer.Infrastructure.OpenApi;
using Eiromplays.IdentityServer.Infrastructure.Persistence;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Initialization;
using Eiromplays.IdentityServer.Infrastructure.SecurityHeaders;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Infrastructure.Test")]

namespace Eiromplays.IdentityServer.Infrastructure;

public static partial class Startup
{
    public static IServiceCollection  AddInfrastructure(this IServiceCollection services, IConfiguration config, ProjectType projectType)
    {
        MapsterSettings.Configure();
        return services
            .RegisterAccountConfiguration(config)
            .AddAuth(config, projectType)
            .AddBackgroundJobs(config)
            .AddCaching(config)
            .AddCorsPolicy(config)
            .AddExceptionMiddleware()
            .AddHealthCheck()
            .AddPOLocalization(config)
            .AddMailing(config)
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddMultitenancy(config)
            .AddNotifications(config)
            .AddOpenApiDocumentation(config)
            .AddPersistence(config)
            .AddRequestLogging(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddServices();
    }

    private static IServiceCollection AddHealthCheck(this IServiceCollection services) =>
        services.AddHealthChecks().AddCheck<TenantHealthCheck>("Tenant").Services;

    public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        // Create a new scope to retrieve scoped services
        using var scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
            .InitializeDatabasesAsync(cancellationToken);
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
        builder
            .UseRequestLocalization()
            .UseStaticFiles()
            .UseSecurityHeaders(config)
            .UseFileStorage()
            .UseExceptionMiddleware()
            .UseRouting()
            .UseCorsPolicy()
            .UseAuthentication()
            .UseCurrentUser()
            .UseMultiTenancy()
            .UseAuthorization()
            .UseRequestLogging(config)
            .UseHangfireDashboard(config)
            .UseOpenApiDocumentation(config);

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers().RequireAuthorization();
        builder.MapHealthCheck();
        builder.MapNotifications();
        return builder;
    }

    private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapHealthChecks("/api/health").RequireAuthorization();
    
    /*
        Registers the Account Configuration
        Information:
        Profile Picture Configuration:
        Find more avatar styles here: https://avatars.dicebear.com/styles/
        You can also use a custom provider
    */
    private static IServiceCollection RegisterAccountConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<AccountConfiguration>(configuration.GetSection(nameof(AccountConfiguration)));
    }
}