namespace Eiromplays.IdentityServer.Application.Auditing;

public interface IAuditService : ITransientService
{
    Task<List<AuditDto>> GetUserTrailsAsync(string userId);

    Task<AuditDto> GetTrailAsync(string id, CancellationToken cancellationToken = default);
}