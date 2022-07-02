using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Initialization;

internal class DatabaseInitializer : IDatabaseInitializer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(IServiceProvider serviceProvider, ILogger<DatabaseInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task InitializeDatabasesAsync(CancellationToken cancellationToken)
    {
        await InitializeApplicationDbAsync(cancellationToken);
        await InitializeSessionDbAsync(cancellationToken);

        _logger.LogInformation("For documentations and guides, visit https://www.fullstackhero.net");
        _logger.LogInformation("To Sponsor this project, visit https://opencollective.com/fullstackhero");
    }

    private async Task InitializeApplicationDbAsync(CancellationToken cancellationToken)
    {
        // First create a new scope
        using var scope = _serviceProvider.CreateScope();

        // Then run the initialization in the new scope
        var applicationDbInitializer = scope.ServiceProvider.GetService<ApplicationDbInitializer>();
        if (applicationDbInitializer is not null)
                await applicationDbInitializer.InitializeAsync(cancellationToken);
    }

    private async Task InitializeSessionDbAsync(CancellationToken cancellationToken)
    {
        // First create a new scope
        using var scope = _serviceProvider.CreateScope();

        // Then run the initialization in the new scope
        await scope.ServiceProvider.GetRequiredService<SessionDbInitializer>()
            .InitializeAsync(cancellationToken);
    }
}