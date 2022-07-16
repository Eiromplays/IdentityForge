namespace Eiromplays.IdentityServer.Application.Identity.ApiResources;

public interface IApiResourceService : ITransientService
{
    Task<PaginationResponse<ApiResourceDto>> SearchAsync(ApiResourceListFilter filter, CancellationToken cancellationToken = default);

    Task<ApiResourceDto> GetAsync(int apiResourceId, CancellationToken cancellationToken = default);

    Task UpdateAsync(UpdateApiResourceRequest request, int apiResourceId, CancellationToken cancellationToken = default);

    Task DeleteAsync(int apiResourceId, CancellationToken cancellationToken = default);

    Task<string> CreateAsync(CreateApiResourceRequest request, CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(CancellationToken cancellationToken);
}