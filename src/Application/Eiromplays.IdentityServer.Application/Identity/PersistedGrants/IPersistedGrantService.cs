namespace Eiromplays.IdentityServer.Application.Identity.PersistedGrants;

public interface IPersistedGrantService : ITransientService
{
    Task<List<PersistedGrantDto>> GetListAsync(CancellationToken cancellationToken);
}