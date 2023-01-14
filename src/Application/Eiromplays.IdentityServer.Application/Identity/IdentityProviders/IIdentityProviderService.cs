namespace Eiromplays.IdentityServer.Application.Identity.IdentityProviders;

public interface IIdentityProviderService : ITransientService
{
    Task<PaginationResponse<IdentityProviderDto>> SearchAsync(IdentityProviderListFilter filter, CancellationToken cancellationToken = default);

    Task<IdentityProviderDto> GetAsync(int identityProviderId, CancellationToken cancellationToken = default);

    Task UpdateAsync(UpdateIdentityProviderRequest request, int identityProviderId, CancellationToken cancellationToken = default);

    Task DeleteAsync(int identityProviderId, CancellationToken cancellationToken = default);

    Task<string> CreateAsync(CreateIdentityProviderRequest request, CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
}