using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;

namespace IdentityForge.IdentityServer.Database;

public class MigrationHostedService<TDbContext> : IHostedService
    where TDbContext : DbContext
{
    private readonly ILogger<MigrationHostedService<TDbContext>> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public MigrationHostedService(IServiceScopeFactory scopeFactory, ILogger<MigrationHostedService<TDbContext>> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Applying migrations for {DbContext}", typeof(TDbContext).Name);

            await using var scope = _scopeFactory.CreateAsyncScope();
            await using var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

            var sp = context.GetInfrastructure();

            var modelDiffer = sp.GetRequiredService<IMigrationsModelDiffer>();
            var migrationsAssembly = sp.GetRequiredService<IMigrationsAssembly>();

            if (migrationsAssembly.ModelSnapshot is null)
            {
                _logger.LogWarning("No model snapshot found for {DbContext}", typeof(TDbContext).Name);
                return;
            }

            var modelInitializer = sp.GetRequiredService<IModelRuntimeInitializer>();
            var sourceModel = modelInitializer.Initialize(migrationsAssembly.ModelSnapshot.Model);

            var designTimeModel = sp.GetRequiredService<IDesignTimeModel>();
            var readOptimizedModel = designTimeModel.Model;

            bool diffsExist = modelDiffer.HasDifferences(
                sourceModel.GetRelationalModel(),
                readOptimizedModel.GetRelationalModel());

            if(diffsExist) {
                throw new InvalidOperationException("There are differences between the current database model and the most recent migration.");
            }

            await context.Database.MigrateAsync(cancellationToken: cancellationToken);

            // https://www.npgsql.org/efcore/mapping/enum.html#creating-your-database-enum
            await using var conn = (NpgsqlConnection)context.Database.GetDbConnection();
            await conn.OpenAsync(cancellationToken);
            await conn.ReloadTypesAsync();

            _logger.LogInformation("Migrations complete for {DbContext}", typeof(TDbContext).Name);
        }
        catch (Exception ex) when (ex is SocketException or NpgsqlException)
        {
            _logger.LogError(ex, "Could not connect to the database. Please check the connection string and make sure the database is running");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while applying the database migrations");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}