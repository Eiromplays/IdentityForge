using Eiromplays.IdentityServer.Application.Common.Configurations.Database;
using Eiromplays.IdentityServer.Application.Multitenancy;
using Eiromplays.IdentityServer.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Shared.Authorization;
using Shared.Multitenancy;

namespace Eiromplays.IdentityServer.Infrastructure.Multitenancy;

internal static class Startup
{
    internal static IServiceCollection AddMultitenancy(this IServiceCollection services, IConfiguration config)
    {
        var databaseConfiguration = config.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();

        if (string.IsNullOrEmpty(databaseConfiguration.ConnectionStringsConfiguration.TenantDbConnection)) 
            throw new InvalidOperationException("DB ConnectionString is not configured.");
        

        return services
            .AddDbContext<TenantDbContext>(m => m.UseDatabase(databaseConfiguration.DatabaseProvider,
                databaseConfiguration.ConnectionStringsConfiguration.TenantDbConnection))
            .AddMultiTenant<EIATenantInfo>()
                .WithClaimStrategy(EIAClaims.Tenant)
                .WithHeaderStrategy(MultitenancyConstants.TenantIdName)
                .WithQueryStringStrategy(MultitenancyConstants.TenantIdName)
                .WithEFCoreStore<TenantDbContext, EIATenantInfo>()
                .Services
            .AddScoped<ITenantService, TenantService>();
    }

    internal static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app) =>
        app.UseMultiTenant();

    private static FinbuckleMultiTenantBuilder<EIATenantInfo> WithQueryStringStrategy(this FinbuckleMultiTenantBuilder<EIATenantInfo> builder, string queryStringKey) =>
        builder.WithDelegateStrategy(context =>
        {
            if (context is not HttpContext httpContext)
            {
                return Task.FromResult((string?)null);
            }
            httpContext.Request.Query.TryGetValue(queryStringKey, out var tenantIdParam);

            return Task.FromResult((string?)tenantIdParam.ToString());
        });
}