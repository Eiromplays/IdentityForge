namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Initialization;

internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    Task InitializeApplicationDbAsync(CancellationToken cancellationToken);
    Task InitializeIdentityServerConfigurationDbAsync(CancellationToken cancellationToken);
    Task InitializeIdentityServerPersistedGrantDbAsync(CancellationToken cancellationToken);
}