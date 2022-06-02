using System.Reflection;
using System.Runtime.CompilerServices;
using Duende.Bff;
using Duende.Bff.Yarp;
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
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Infrastructure.Test")]

namespace Eiromplays.IdentityServer.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config, ProjectType projectType)
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
            .AddPOLocalization(config)
            .AddMailing(config)
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddNotifications(config)
            .AddOpenApiDocumentation(config, projectType)
            .AddPersistence(config, projectType)
            .AddDataProtection().Services
            .AddAuth(config, projectType)
            .AddRequestLogging(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddServices();
    }

    private static IServiceCollection AddInfrastructureSpa(this IServiceCollection services, IConfiguration config, ProjectType projectType)
    {
        if (projectType is not ProjectType.Spa) return services;

        var bffBuilder = services.AddBff(options => config.GetSection(nameof(BffOptions)).Bind(options));

        bffBuilder.AddRemoteApis();
        bffBuilder.AddBffPersistence(config, projectType);

        return services;
    }
    
    private static IServiceCollection AddHealthCheck(this IServiceCollection services) =>
        services.AddHealthChecks().Services;

    public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        // Create a new scope to retrieve scoped services
        using var scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
            .InitializeDatabasesAsync(cancellationToken);
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config, ProjectType projectType)
    {
        if (projectType is ProjectType.Spa) return builder;
        
        return builder
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
            .UseHangfireDashboard(config)
            .UseOpenApiDocumentation(config, projectType);
    }

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
        Configures custom classes for config files, so they can be retrieved from DI using IOptions<T>
        Information:
        Account Configuration: 
        Profile Picture Configuration:
        Find more avatar styles here: https://avatars.dicebear.com/styles/
        You can also use a custom provider
    */
    private static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<AccountConfiguration>(configuration.GetSection(nameof(AccountConfiguration)))
            .Configure<IdentityServerData>(configuration.GetSection(nameof(IdentityServerData)))
            .Configure<IdentityData>(configuration.GetSection(nameof(IdentityData)));
    }

    private static IApplicationBuilder UseIdentityServer(this IApplicationBuilder builder, ProjectType projectType)
    {
        return projectType is ProjectType.IdentityServer ? builder.UseIdentityServer() : builder;
    }
}