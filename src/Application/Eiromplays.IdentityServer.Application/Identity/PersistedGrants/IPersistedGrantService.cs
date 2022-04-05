namespace Eiromplays.IdentityServer.Application.Identity.PersistedGrants;

public interface IPersistedGrantService : ITransientService
{
    Task<PaginationResponse<PersistedGrantDto>> SearchAsync(PersistedGrantListFilter filter,
        CancellationToken cancellationToken = default);
    
    Task<List<PersistedGrantDto>> GetListAsync(CancellationToken cancellationToken = default);

    Task<PersistedGrantDto> GetAsync(string key, CancellationToken cancellationToken = default);

    Task<List<PersistedGrantDto>> GetUserPersistedGrantsAsync(string subjectId, CancellationToken cancellationToken = default);

    Task<string> DeleteAsync(string key, CancellationToken cancellationToken = default);

    Task<string> DeleteUserPersistedGrantsAsync(string subjectId, CancellationToken cancellationToken = default);
}