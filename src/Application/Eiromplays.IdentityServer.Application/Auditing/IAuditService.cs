using Eiromplays.IdentityServer.Application.Common.Interfaces;

namespace Eiromplays.IdentityServer.Application.Auditing;

public interface IAuditService : ITransientService
{
    Task<List<AuditDto>> GetUserTrailsAsync(string userId);
}