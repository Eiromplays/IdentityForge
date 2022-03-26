using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Initialization;

internal class IdentityServerPersistedGrantDbInitializer
{
    private readonly IdentityServerPersistedGrantDbContext _dbContext;
    private readonly ITenantInfo _currentTenant;
    private readonly ILogger<IdentityServerPersistedGrantDbInitializer> _logger;

    public IdentityServerPersistedGrantDbInitializer(IdentityServerPersistedGrantDbContext dbContext,
        ITenantInfo currentTenant,
        ILogger<IdentityServerPersistedGrantDbInitializer> logger)
    {
        _dbContext = dbContext;
        _currentTenant = currentTenant;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (_dbContext.Database.GetMigrations().Any())
        {
            if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                _logger.LogInformation("Applying Migrations for '{TenantId}' tenant.", _currentTenant.Id);
                await _dbContext.Database.MigrateAsync(cancellationToken);
            }
        }
    }
}