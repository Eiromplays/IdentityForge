using Ardalis.Specification.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Mappers;
using Eiromplays.IdentityServer.Application.Common.Events;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Common.Specification;
using Eiromplays.IdentityServer.Application.Identity.Clients;
using Eiromplays.IdentityServer.Domain.Identity;
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
    private readonly IEventPublisher _events;

    public ClientService(ApplicationDbContext db, IStringLocalizer<ClientService> t, ILogger<ClientService> logger,
        IEventPublisher events)
    {
        _db = db;
        _t = t;
        _logger = logger;
        _events = events;
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
    
    public async Task<Duende.IdentityServer.Models.Client> GetAsync(string? clientId, CancellationToken cancellationToken)
    {
        var client = await _db.Clients
            .Where(x => x.ClientId == clientId)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);

        _ = client ?? throw new NotFoundException(_t["Client Not Found."]);

        var model = client.ToModel();
        
        return model.Adapt<Duende.IdentityServer.Models.Client>();
    }
    
    public async Task UpdateAsync(UpdateClientRequest request, string clientId, CancellationToken cancellationToken)
    {
        var client = await GetAsync(clientId, cancellationToken);

        _ = client ?? throw new NotFoundException(_t["Client Not Found."]);

        client.ClientName = request.ClientName;
        client.Description = request.Description;

        var clientEntity = client.ToEntity();
        
        _db.Clients.Update(clientEntity);
        
        var success = await _db.SaveChangesAsync(cancellationToken) > 0;

        await _events.PublishAsync(new ClientUpdatedEvent(client.ClientId));
        
        if (!success)
        {
            throw new InternalServerException(_t["Update client failed"], new List<string>{ "Failed to update client" });
        }
    }
}