using Ardalis.Specification.EntityFrameworkCore;
using Eiromplays.IdentityServer.Application.Auditing;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Common.Specification;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Eiromplays.IdentityServer.Infrastructure.Auditing;

public class AuditService : IAuditService
{
    private readonly ApplicationDbContext _context;
    private readonly IStringLocalizer _t;

    public AuditService(ApplicationDbContext context, IStringLocalizer<AuditService> t)
    {
        _context = context;
        _t = t;
    }
    
    public async Task<PaginationResponse<AuditDto>> SearchAsync(AuditLogListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<Trail>(filter);

        var trails = await _context.AuditTrails
            .WithSpecification(spec)
            .ProjectToType<AuditDto>()
            .ToListAsync(cancellationToken);
        
        var count = await _context.AuditTrails
            .CountAsync(cancellationToken);

        return new PaginationResponse<AuditDto>(trails, count, filter.PageNumber, filter.PageSize);
    }
    
    public async Task<List<AuditDto>> GetUserTrailsAsync(string userId)
    {
        var trails = await _context.AuditTrails
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.DateTime)
            .Take(250)
            .ToListAsync();

        return trails.Adapt<List<AuditDto>>();
    }
    
    public async Task<AuditDto> GetTrailAsync(string id, CancellationToken cancellationToken)
    {
        var trail = await _context.AuditTrails
            .Where(a => a.Id.ToString().Equals(id))
            .FirstOrDefaultAsync(cancellationToken);

        _ = trail ?? throw new NotFoundException(_t["Trail Not Found."]);
        
        return trail.Adapt<AuditDto>();
    }
    
    public async Task<List<AuditDto>> GetListAsync(CancellationToken cancellationToken)
    {
        var trails = await _context.AuditTrails.OrderByDescending(a => a.DateTime).ToListAsync(cancellationToken);

        return trails.Adapt<List<AuditDto>>();
    }
}