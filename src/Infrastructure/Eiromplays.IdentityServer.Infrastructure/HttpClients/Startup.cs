using Eiromplays.IdentityServer.Application.Common.Configurations;
using Eiromplays.IdentityServer.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace Eiromplays.IdentityServer.Infrastructure.HttpClients;

internal static class Startup
{
    internal static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration, ProjectType projectType)
    {
        if (projectType != ProjectType.IdentityServer) return services;

        var urlConfiguration = configuration.GetSection(nameof(UrlConfiguration)).Get<UrlConfiguration>() ??
                               new UrlConfiguration();

        services
            .AddTransient<BearerTokenHandler>()
            .AddHttpClient("Api", client =>
            {
                client.BaseAddress = new Uri(urlConfiguration.ApiBaseUrl);
            })
            .AddHttpMessageHandler<BearerTokenHandler>()
            .AddTransientHttpErrorPolicy(policyBuilder =>
                policyBuilder.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5)));

        return services;
    }
}