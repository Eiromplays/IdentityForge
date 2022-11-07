namespace Eiromplays.IdentityServer.Application.Identity.ApiScopes;

public interface IApiScopeService : ITransientService
{
    Task<Result<PaginationResponse<ApiScopeDto>>> SearchAsync(ApiScopeListFilter filter, CancellationToken cancellationToken = default);

    Task<Result<ApiScopeDto>> GetAsync(int apiScopeId, CancellationToken cancellationToken = default);

    Task UpdateAsync(UpdateApiScopeRequest request, int apiScopeId, CancellationToken cancellationToken = default);

    Task DeleteAsync(int apiScopeId, CancellationToken cancellationToken = default);

    Task<Result<string>> CreateAsync(CreateApiScopeRequest request, CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
}