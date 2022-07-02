using Duende.IdentityServer;
using Eiromplays.IdentityServer.Application.Identity.Resources;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal class ResourceService : IResourceService
{
    private readonly ApplicationDbContext _db;

    public ResourceService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<ApiResourceDto>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames, CancellationToken cancellationToken)
    {
        var query =
            from apiResource in _db.ApiResources
            where apiResourceNames.Contains(apiResource.Name)
            select apiResource;

        var apis = await query
            .Include(x => x.Secrets)
            .Include(x => x.Scopes)
            .Include(x => x.UserClaims)
            .Include(x => x.Properties)
            .AsNoTracking()
            .Where(x => apiResourceNames.Contains(x.Name))
            .ToListAsync(cancellationToken);

        return apis.Adapt<List<ApiResourceDto>>();
    }

    public async Task<List<ApiResourceDto>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames, CancellationToken cancellationToken)
    {
        var query =
            from api in _db.ApiResources
            where api.Scopes.Any(x => scopeNames.Contains(x.Scope))
            select api;

        var apis = await query
            .Include(x => x.Secrets)
            .Include(x => x.Scopes)
            .Include(x => x.UserClaims)
            .Include(x => x.Properties)
            .AsNoTracking()
            .Where(api => api.Scopes.Any(x => scopeNames.Contains(x.Scope)))
            .ToListAsync(cancellationToken);

        return apis.Adapt<List<ApiResourceDto>>();
    }

    public async Task<List<IdentityResourceDto>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames, CancellationToken cancellationToken)
    {
        var query =
            from identityResource in _db.IdentityResources
            where scopeNames.Contains(identityResource.Name)
            select identityResource;

        var resources = await query
            .Include(x => x.UserClaims)
            .Include(x => x.Properties)
            .AsNoTracking()
            .Where(x => scopeNames.Contains(x.Name))
            .ToListAsync(cancellationToken);

        return resources.Adapt<List<IdentityResourceDto>>();
    }

    public async Task<List<ApiScopeDto>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames, CancellationToken cancellationToken)
    {
        var query =
            from scope in _db.ApiScopes
            where scopeNames.Contains(scope.Name)
            select scope;

        var resources = await query
            .Include(x => x.UserClaims)
            .Include(x => x.Properties)
            .AsNoTracking()
            .Where(x => scopeNames.Contains(x.Name))
            .ToListAsync(cancellationToken);

        return resources.Adapt<List<ApiScopeDto>>();
    }

    public async Task<ResourcesDto> FindResourcesByScopeAsync(IEnumerable<string> scopeNames, CancellationToken cancellationToken)
    {
        scopeNames = scopeNames.ToList();
        var identity = await FindIdentityResourcesByScopeNameAsync(scopeNames, cancellationToken);
        var apiResources = await FindApiResourcesByScopeNameAsync(scopeNames, cancellationToken);
        var scopes = await FindApiScopesByNameAsync(scopeNames, cancellationToken);

        var resources = new ResourcesDto(identity, apiResources, scopes)
        {
            OfflineAccess = scopeNames.Contains(IdentityServerConstants.StandardScopes.OfflineAccess)
        };

        return resources;
    }
}