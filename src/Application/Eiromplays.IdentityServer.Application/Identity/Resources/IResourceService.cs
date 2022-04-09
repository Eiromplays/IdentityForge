namespace Eiromplays.IdentityServer.Application.Identity.Resources;

public interface IResourceService : ITransientService
{
    Task<ResourcesDto> FindResourcesByScopeAsync(IEnumerable<string> scopeNames, CancellationToken cancellationToken = default);
}