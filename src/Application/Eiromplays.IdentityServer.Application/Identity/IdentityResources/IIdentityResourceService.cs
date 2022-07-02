namespace Eiromplays.IdentityServer.Application.Identity.IdentityResources;

public interface IIdentityResourceService : ITransientService
{
    Task<PaginationResponse<IdentityResourceDto>> SearchAsync(IdentityResourceListFilter filter, CancellationToken cancellationToken = default);

    Task<IdentityResourceDto> GetAsync(int identityResourceId, CancellationToken cancellationToken = default);

    Task UpdateAsync(UpdateIdentityResourceRequest request, int identityResourceId, CancellationToken cancellationToken = default);

    Task DeleteAsync(int identityResourceId, CancellationToken cancellationToken = default);

    Task<string> CreateAsync(CreateIdentityResourceRequest request, CancellationToken cancellationToken = default);
}