using Ardalis.Specification.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.Entities;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Common.Specification;
using Eiromplays.IdentityServer.Application.Identity.PersistedGrants;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class PersistedGrantService : IPersistedGrantService
{
    private readonly ApplicationDbContext _db;

    public PersistedGrantService(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<PaginationResponse<PersistedGrantDto>> SearchAsync(PersistedGrantListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<PersistedGrant>(filter);

        var persistedGrants = await _db.PersistedGrants
            .WithSpecification(spec)
            .ProjectToType<PersistedGrantDto>()
            .ToListAsync(cancellationToken);
        
        var count = await _db.PersistedGrants
            .CountAsync(cancellationToken);

        return new PaginationResponse<PersistedGrantDto>(persistedGrants, count, filter.PageNumber, filter.PageSize);
    }
    
    public async Task<List<PersistedGrantDto>> GetListAsync(CancellationToken cancellationToken) =>
        (await _db.PersistedGrants
            .AsNoTracking()
            .ToListAsync(cancellationToken))
        .Adapt<List<PersistedGrantDto>>();
}