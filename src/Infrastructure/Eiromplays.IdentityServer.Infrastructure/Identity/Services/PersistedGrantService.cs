using Ardalis.Specification.EntityFrameworkCore;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Common.Specification;
using Eiromplays.IdentityServer.Application.Identity.PersistedGrants;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using PersistedGrant = Duende.IdentityServer.EntityFramework.Entities.PersistedGrant;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal class PersistedGrantService : IPersistedGrantService
{
    private readonly ApplicationDbContext _db;
    private readonly IStringLocalizer _t;

    public PersistedGrantService(ApplicationDbContext db, IStringLocalizer<PersistedGrantService> t)
    {
        _db = db;
        _t = t;
    }

    public async Task<PaginationResponse<PersistedGrantDto>> SearchAsync(PersistedGrantListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<PersistedGrant>(filter);

        var persistedGrants = await _db.PersistedGrants
            .WithSpecification(spec)
            .ProjectToType<PersistedGrantDto>()
            .ToListAsync(cancellationToken);

        int count = await _db.PersistedGrants
            .CountAsync(cancellationToken);

        return new PaginationResponse<PersistedGrantDto>(persistedGrants, count, filter.PageNumber, filter.PageSize);
    }

    public async Task<List<PersistedGrantDto>> GetListAsync(CancellationToken cancellationToken) =>
        (await _db.PersistedGrants
            .AsNoTracking()
            .ToListAsync(cancellationToken))
        .Adapt<List<PersistedGrantDto>>();

    public async Task<PersistedGrantDto> GetAsync(string key, CancellationToken cancellationToken)
    {
        var persistedGrant = await _db.PersistedGrants
            .AsNoTracking()
            .Where(u => u.Key == key)
            .FirstOrDefaultAsync(cancellationToken);

        _ = persistedGrant ?? throw new NotFoundException(_t["Persisted Grant Not Found."]);

        return persistedGrant.Adapt<PersistedGrantDto>();
    }

    public async Task<List<PersistedGrantDto>> GetUserPersistedGrantsAsync(string subjectId, CancellationToken cancellationToken) =>
        (await _db.PersistedGrants
            .AsNoTracking()
            .Where(u => u.SubjectId == subjectId)
            .ToListAsync(cancellationToken)).Adapt<List<PersistedGrantDto>>();

    public async Task<string> DeleteAsync(string key, CancellationToken cancellationToken)
    {
        var persistedGrant = await _db.PersistedGrants
            .AsNoTracking()
            .Where(u => u.Key == key)
            .FirstOrDefaultAsync(cancellationToken);

        _ = persistedGrant ?? throw new NotFoundException(_t["Persisted Grant Not Found."]);

        _db.PersistedGrants.Remove(persistedGrant);

        await _db.SaveChangesAsync(cancellationToken);

        return string.Format(_t["Persisted Grant {0} Deleted."], persistedGrant.Key);
    }

    public async Task<string> DeleteUserPersistedGrantsAsync(string subjectId, CancellationToken cancellationToken)
    {
        var persistedGrants = await _db.PersistedGrants
            .AsNoTracking()
            .Where(u => u.SubjectId == subjectId)
            .ToListAsync(cancellationToken);

        if (!persistedGrants.Any())
            throw new NotFoundException(_t["Persisted Grant Not Found."]);

        _db.PersistedGrants.RemoveRange(persistedGrants);

        await _db.SaveChangesAsync(cancellationToken);

        return string.Format(_t["Persisted Grant {0} User Deleted."], subjectId);
    }
}