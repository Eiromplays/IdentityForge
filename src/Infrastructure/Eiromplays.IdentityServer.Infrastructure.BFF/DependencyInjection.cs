using Duende.Bff;
using Duende.Bff.Yarp;
using Eiromplays.IdentityServer.Application.Common.Configurations.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eiromplays.IdentityServer.Infrastructure.BFF;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConfiguration =
            configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();
        
        var bffBuilder = services.AddBff(options => configuration.GetSection(nameof(BffOptions)).Bind(options));

        bffBuilder.AddRemoteApis();
        bffBuilder.RegisterDbContexts(databaseConfiguration);

        return services;
    }
}