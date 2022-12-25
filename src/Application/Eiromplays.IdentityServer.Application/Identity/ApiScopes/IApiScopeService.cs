namespace Eiromplays.IdentityServer.Application.Identity.ApiScopes;

public interface IApiScopeService : ITransientService
{
    Task<PaginationResponse<ApiScopeDto>> SearchAsync(ApiScopeListFilter filter, CancellationToken cancellationToken = default);

    Task<ApiScopeDto> GetAsync(int apiScopeId, CancellationToken cancellationToken = default);

    Task UpdateAsync(UpdateApiScopeRequest request, int apiScopeId, CancellationToken cancellationToken = default);

    Task DeleteAsync(int apiScopeId, CancellationToken cancellationToken = default);

    Task<string> CreateAsync(CreateApiScopeRequest request, CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
}