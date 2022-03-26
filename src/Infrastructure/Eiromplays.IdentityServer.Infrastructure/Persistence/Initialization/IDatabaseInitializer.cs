using Eiromplays.IdentityServer.Infrastructure.Multitenancy;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Initialization;

internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    Task InitializeApplicationDbForTenantAsync(EIATenantInfo tenant, CancellationToken cancellationToken);
}