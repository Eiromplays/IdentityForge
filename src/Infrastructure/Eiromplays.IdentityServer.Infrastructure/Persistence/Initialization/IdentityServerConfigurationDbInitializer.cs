using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Initialization;

internal class IdentityServerConfigurationDbInitializer
{
    private readonly IdentityServerConfigurationDbContext _dbContext;
    private readonly IdentityServerConfigurationDbSeeder _dbSeeder;
    private readonly ILogger<IdentityServerConfigurationDbInitializer> _logger;

    public IdentityServerConfigurationDbInitializer(IdentityServerConfigurationDbContext dbContext,
        IdentityServerConfigurationDbSeeder dbSeeder,
        ILogger<IdentityServerConfigurationDbInitializer> logger)
    {
        _dbContext = dbContext;
        _dbSeeder = dbSeeder;
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

            if (await _dbContext.Database.CanConnectAsync(cancellationToken))
            {
                _logger.LogInformation("Connection to Database Succeeded.");

                await _dbSeeder.SeedDatabaseAsync(_dbContext, cancellationToken);
            }
        }
    }
}