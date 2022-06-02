using Duende.Bff.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Initialization;

internal class SessionDbInitializer
{
    private readonly SessionDbContext _dbContext;
    private readonly ILogger<SessionDbInitializer> _logger;

    public SessionDbInitializer(SessionDbContext dbContext, ILogger<SessionDbInitializer> logger)
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
                _logger.LogInformation("Applying Migrations");
                await _dbContext.Database.MigrateAsync(cancellationToken);
            }
        }
    }
}
