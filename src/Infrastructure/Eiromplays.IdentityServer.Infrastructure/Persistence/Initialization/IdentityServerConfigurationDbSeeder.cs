using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Eiromplays.IdentityServer.Application.Common.Configurations.Identity;
using Eiromplays.IdentityServer.Infrastructure.Multitenancy;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Initialization;

internal class IdentityServerConfigurationDbSeeder
{
    private readonly EIATenantInfo _currentTenant;
    private readonly CustomSeederRunner _seederRunner;
    private readonly ILogger<IdentityServerConfigurationDbSeeder> _logger;
    private readonly IdentityServerData _identityServerData;

    public IdentityServerConfigurationDbSeeder(EIATenantInfo currentTenant, CustomSeederRunner seederRunner,
        ILogger<IdentityServerConfigurationDbSeeder> logger, IOptionsMonitor<IdentityServerData> identityServerData)
    {
        _currentTenant = currentTenant;
        _seederRunner = seederRunner;
        _logger = logger;
        _identityServerData = identityServerData.CurrentValue;
    }

    public async Task SeedDatabaseAsync(IdentityServerConfigurationDbContext dbContext, CancellationToken cancellationToken)
    {
        await SeedIdentityResources(dbContext);
        await SeedApiScopes(dbContext);
        await SeedClients(dbContext);
        await SeedApiResources(dbContext);
        await _seederRunner.RunSeedersAsync(cancellationToken);
    }

    private async Task SeedIdentityResources(IdentityServerConfigurationDbContext dbContext)
    {
        foreach (var resource in _identityServerData.IdentityResources)
        {
            var exits = await dbContext.IdentityResources.AnyAsync(a => a.Name == resource.Name);

            if (exits)
            {
                continue;
            }
            
            _logger.LogInformation("Seeding {Resource} Resource for '{TenantId}' Tenant", resource.Name, _currentTenant.Id);
            await dbContext.IdentityResources.AddAsync(resource.ToEntity());
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task SeedApiScopes(IdentityServerConfigurationDbContext dbContext)
    {
        foreach (var apiScope in _identityServerData.ApiScopes)
        {
            var exits = await dbContext.ApiScopes.AnyAsync(a => a.Name == apiScope.Name);

            if (exits)
            {
                continue;
            }

            await dbContext.ApiScopes.AddAsync(apiScope.ToEntity());
        }
        
        await dbContext.SaveChangesAsync();
    }

    private async Task SeedApiResources(IdentityServerConfigurationDbContext dbContext)
    {
        foreach (var resource in _identityServerData.ApiResources)
        {
            var exits = await dbContext.ApiResources.AnyAsync(a => a.Name == resource.Name);

            if (exits)
            {
                continue;
            }

            foreach (var s in resource.ApiSecrets)
            {
                s.Value = s.Value.ToSha256();
            }

            await dbContext.ApiResources.AddAsync(resource.ToEntity());
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task SeedClients(IdentityServerConfigurationDbContext dbContext)
    {
        foreach (var client in _identityServerData.Clients)
        {
            var exits = await dbContext.Clients.AnyAsync(a => a.ClientId == client.ClientId);

            if (exits)
            {
                continue;
            }

            foreach (var secret in client.ClientSecrets)
            {
                secret.Value = secret.Value.ToSha256();
            }

            client.Claims = client.ClientClaims
                .Select(c => new ClientClaim(c.Type, c.Value))
                .ToList();

            await dbContext.Clients.AddAsync(client.ToEntity());
        }

        await dbContext.SaveChangesAsync();
    }
}