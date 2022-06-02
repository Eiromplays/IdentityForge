using System.Data;
using Dapper;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Persistence;
using Eiromplays.IdentityServer.Domain.Common.Contracts;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Repository;

public class DapperRepository : IDapperRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DapperRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    where T : class, IEntity =>
        (await _dbContext.Connection.QueryAsync<T>(sql, param, transaction))
            .AsList();

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    where T : class, IEntity
    {
        var entity = await _dbContext.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);

        return entity ?? throw new NotFoundException(string.Empty);
    }

    public Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    where T : class, IEntity
    {
        return _dbContext.Connection.QuerySingleAsync<T>(sql, param, transaction);
    }
}