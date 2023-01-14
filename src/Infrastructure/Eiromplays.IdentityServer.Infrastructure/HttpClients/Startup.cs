using Eiromplays.IdentityServer.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace Eiromplays.IdentityServer.Infrastructure.HttpClients;

internal static class Startup
{
    internal static IServiceCollection AddHttpClients(this IServiceCollection services, ProjectType projectType)
    {
        if (projectType != ProjectType.IdentityServer) return services;

        services
            .AddTransient<BearerTokenHandler>()
            .AddHttpClient("Api", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7003/");
            })
            .AddHttpMessageHandler<BearerTokenHandler>()
            .AddTransientHttpErrorPolicy(policyBuilder =>
                policyBuilder.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5)));

        return services;
    }
}