using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Initialization;

internal class IdentityServerPersistedGrantDbInitializer
{
    private readonly IdentityServerPersistedGrantDbContext _dbContext;
    private readonly ILogger<IdentityServerPersistedGrantDbInitializer> _logger;

    public IdentityServerPersistedGrantDbInitializer(IdentityServerPersistedGrantDbContext dbContext,
        ILogger<IdentityServerPersistedGrantDbInitializer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (_dbContext.Database.GetMigrations().Any())
        {
            if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                _logger.LogInformation("Applying Migrations.");
                await _dbContext.Database.MigrateAsync(cancellationToken);
            }
        }
    }
}