using Eiromplays.IdentityServer.Application.Common.Caching;

namespace Eiromplays.IdentityServer.Infrastructure.Caching;

public class CacheKeyService : ICacheKeyService
{
    public string GetCacheKey(string name, object id)
    {
        return $"GLOBAL-{name}-{id}";
    }
}