using Ardalis.Specification.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Mappers;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Common.Specification;
using Eiromplays.IdentityServer.Application.Identity.Clients;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class ClientService : IClientService
{
    private readonly ApplicationDbContext _db;
    private readonly IStringLocalizer _t;
    private readonly ILogger _logger;

    public ClientService(ApplicationDbContext db, IStringLocalizer<ClientService> t, ILogger<ClientService> logger)
    {
        _db = db;
        _t = t;
        _logger = logger;
    }
    
    public async Task<PaginationResponse<Duende.IdentityServer.Models.Client>> SearchAsync(ClientListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<Client>(filter);

        var clients = await _db.Clients
            .WithSpecification(spec)
            .AsSplitQuery()
            .Select(x => x.ToModel())
            .ToListAsync(cancellationToken);

        var count = await _db.Clients
            .CountAsync(cancellationToken);

        return new PaginationResponse<Duende.IdentityServer.Models.Client>(clients, count, filter.PageNumber, filter.PageSize);
    }
    
    public async Task<ClientDto> GetAsync(string? clientId, CancellationToken cancellationToken)
    {
        var client = await _db.Clients
            .Where(x => x.ClientId == clientId)
            .Include(x => x.AllowedCorsOrigins)
            .Include(x => x.AllowedGrantTypes)
            .Include(x => x.AllowedScopes)
            .Include(x => x.Claims)
            .Include(x => x.ClientSecrets)
            .Include(x => x.IdentityProviderRestrictions)
            .Include(x => x.PostLogoutRedirectUris)
            .Include(x => x.Properties)
            .Include(x => x.RedirectUris)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);

        _ = client ?? throw new NotFoundException(_t["Client Not Found."]);

        var model = client.ToModel();
        
        return model.Adapt<ClientDto>();
    }
}