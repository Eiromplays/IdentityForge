using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Initialization;

internal class IdentityServerConfigurationDbInitializer
{
    private readonly IdentityServerConfigurationDbContext _dbContext;
    private readonly ITenantInfo _currentTenant;
    private readonly IdentityServerConfigurationDbSeeder _dbSeeder;
    private readonly ILogger<IdentityServerConfigurationDbInitializer> _logger;

    public IdentityServerConfigurationDbInitializer(IdentityServerConfigurationDbContext dbContext,
        ITenantInfo currentTenant, IdentityServerConfigurationDbSeeder dbSeeder,
        ILogger<IdentityServerConfigurationDbInitializer> logger)
    {
        _dbContext = dbContext;
        _currentTenant = currentTenant;
        _dbSeeder = dbSeeder;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (_dbContext.Database.GetMigrations().Any())
        {
            if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                _logger.LogInformation("Applying Migrations for '{tenantId}' tenant.", _currentTenant.Id);
                await _dbContext.Database.MigrateAsync(cancellationToken);
            }

            if (await _dbContext.Database.CanConnectAsync(cancellationToken))
            {
                _logger.LogInformation("Connection to {tenantId}'s Database Succeeded.", _currentTenant.Id);

                await _dbSeeder.SeedDatabaseAsync(_dbContext, cancellationToken);
            }
        }
    }
}