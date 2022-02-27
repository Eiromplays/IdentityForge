using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Eiromplays.IdentityServer.Application.Common.Configurations.Identity;
using IdentityModel;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.DbContexts.Seeds;

public static class IdentityServerConfigurationDbContextSeed
{
    public static async Task SeedIdentityServerDataAsync(IdentityServerConfigurationDbContext identityServerConfigurationDbContext, IdentityServerData identityServerDataConfiguration)
    {
        foreach (var resource in identityServerDataConfiguration.IdentityResources)
        {
            var exits = await identityServerConfigurationDbContext.IdentityResources.AnyAsync(a => a.Name == resource.Name);

            if (exits)
            {
                continue;
            }

            await identityServerConfigurationDbContext.IdentityResources.AddAsync(resource.ToEntity());
        }

        foreach (var apiScope in identityServerDataConfiguration.ApiScopes)
        {
            var exits = await identityServerConfigurationDbContext.ApiScopes.AnyAsync(a => a.Name == apiScope.Name);

            if (exits)
            {
                continue;
            }

            await identityServerConfigurationDbContext.ApiScopes.AddAsync(apiScope.ToEntity());
        }

        foreach (var resource in identityServerDataConfiguration.ApiResources)
        {
            var exits = await identityServerConfigurationDbContext.ApiResources.AnyAsync(a => a.Name == resource.Name);

            if (exits)
            {
                continue;
            }

            foreach (var s in resource.ApiSecrets)
            {
                s.Value = s.Value.ToSha256();
            }

            await identityServerConfigurationDbContext.ApiResources.AddAsync(resource.ToEntity());
        }


        foreach (var client in identityServerDataConfiguration.Clients)
        {
            var exits = await identityServerConfigurationDbContext.Clients.AnyAsync(a => a.ClientId == client.ClientId);

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

            await identityServerConfigurationDbContext.Clients.AddAsync(client.ToEntity());
        }

        await identityServerConfigurationDbContext.SaveChangesAsync();
    }
}