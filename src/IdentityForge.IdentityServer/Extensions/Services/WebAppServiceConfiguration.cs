using System.Reflection;
using System.Text.Json.Serialization;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using IdentityForge.IdentityServer.Middleware;
using IdentityForge.IdentityServer.Services;

namespace IdentityForge.IdentityServer.Extensions.Services;

public static class WebAppServiceConfiguration
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        builder.Services.AddSingleton(Log.Logger);
        builder.Services.AddProblemDetails(ProblemDetailsConfigurationExtension.ConfigureProblemDetails)
            .AddProblemDetailsConventions();

        // TODO update CORS for your env
        builder.Services.AddCorsService("IdentityForgeCorsPolicy", builder.Environment);
        builder.OpenTelemetryRegistration(builder.Configuration, "IdentityForge");
        builder.Services.AddInfrastructure(builder.Environment, builder.Configuration);

        builder.Services.AddFastEndpoints();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // registers all services that inherit from your base service interface - IRecipeManagementScopedService
        builder.Services.AddBoundaryServices(Assembly.GetExecutingAssembly());

        builder.Services.AddMvc();

        builder.Services.AddHealthChecks();
        builder.Services.AddSwaggerExtension(builder.Configuration);
    }

    /// <summary>
    /// Registers all services in the assembly of the given interface.
    /// </summary>
    private static void AddBoundaryServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (!assemblies.Any())
            throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");

        foreach (var assembly in assemblies)
        {
            var rules = assembly.GetTypes()
                .Where(x => x is { IsAbstract: false, IsClass: true } && x.GetInterface(nameof(IIdentityForgeScopedService)) == typeof(IIdentityForgeScopedService));

            foreach (var rule in rules)
            {
                foreach (var @interface in rule.GetInterfaces())
                {
                    services.Add(new ServiceDescriptor(@interface, rule, ServiceLifetime.Scoped));
                }
            }
        }
    }
}
