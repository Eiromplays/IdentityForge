namespace Eiromplays.IdentityServer.Application.Auditing;

public interface IAuditService : ITransientService
{
    Task<PaginationResponse<AuditDto>> SearchAsync(AuditLogListFilter filter, CancellationToken cancellationToken = default);

    Task<List<AuditDto>> GetUserTrailsAsync(string userId, CancellationToken cancellationToken = default);

    Task<AuditDto> GetTrailAsync(string id, CancellationToken cancellationToken = default);

    Task<List<AuditDto>> GetListAsync(CancellationToken cancellationToken = default);
}