using Ardalis.Specification.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.Entities;
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

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal class ClientService : IClientService
{
    private readonly ApplicationDbContext _db;
    private readonly IStringLocalizer _t;
    private readonly IEventPublisher _events;

    public ClientService(ApplicationDbContext db, IStringLocalizer<ClientService> t, IEventPublisher events)
    {
        _db = db;
        _t = t;
        _events = events;
    }

    public async Task<PaginationResponse<ClientDto>> SearchAsync(ClientListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<Client>(filter);

        var clients = await _db.Clients
            .WithSpecification(spec)
            .ProjectToType<ClientDto>()
            .ToListAsync(cancellationToken);

        int count = await _db.Clients
            .CountAsync(cancellationToken);

        return new PaginationResponse<ClientDto>(clients, count, filter.PageNumber, filter.PageSize);
    }

    public async Task<ClientDto> GetAsync(int clientId, CancellationToken cancellationToken)
    {
        var client = await FindClientByIdAsync(clientId, cancellationToken);

        return client.Adapt<ClientDto>();
    }

    public async Task UpdateAsync(UpdateClientRequest request, int clientId, CancellationToken cancellationToken)
    {
        var client = await FindClientByIdAsync(clientId, cancellationToken);

        client.ClientId = request.ClientId;
        client.ClientName = request.ClientName;
        client.Description = request.Description;
        client.ClientUri = request.ClientUri;
        client.LogoUri = request.LogoUri;
        client.RequireConsent = request.RequireConsent;
        client.AllowRememberConsent = request.AllowRememberConsent;
        client.Enabled = request.Enabled;

        _db.Clients.Update(client);

        bool success = await _db.SaveChangesAsync(cancellationToken) > 0;

        await _events.PublishAsync(new ClientUpdatedEvent(client.Id));

        if (!success)
        {
            throw new InternalServerException(_t["Update client failed"], new List<string> { "Failed to update client" });
        }
    }

    public async Task DeleteAsync(int clientId, CancellationToken cancellationToken)
    {
        var client = await FindClientByIdAsync(clientId, cancellationToken);

        _db.Clients.Remove(client);

        bool success = await _db.SaveChangesAsync(cancellationToken) > 0;

        await _events.PublishAsync(new ClientDeletedEvent(client.Id));

        if (!success)
        {
            throw new InternalServerException(_t["Delete client failed"], new List<string> { "Failed to delete client" });
        }
    }

    public async Task<string> CreateAsync(CreateClientRequest request, CancellationToken cancellationToken)
    {
        var client = new Client
        {
            ClientId = request.ClientId,
            ClientName = request.ClientName,
            Description = request.Description,
            ClientUri = request.ClientUri,
            LogoUri = request.LogoUri,
            Enabled = request.Enabled,
            RequireConsent = request.RequireConsent,
            AllowRememberConsent = request.AllowRememberConsent
        };

        await _db.Clients.AddAsync(client, cancellationToken);

        bool success = await _db.SaveChangesAsync(cancellationToken) > 0;

        if (!success) throw new InternalServerException(_t["Create client failed"], new List<string> { "Failed to create client" });

        await _events.PublishAsync(new ClientCreatedEvent(client.Id));

        return string.Format(_t["Client {0} Registered."], client.Id);
    }

    public Task<int> GetCountAsync(CancellationToken cancellationToken) =>
        _db.Clients.AsNoTracking().CountAsync(cancellationToken);

    #region Entity Queries

    public async Task<bool> ExistsWithClientIdAsync(string clientId, CancellationToken cancellationToken, int? exceptId = null)
    {
        return await _db.Clients
            .Where(x => x.ClientId == clientId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken) is { } client && client.Id == exceptId;
    }

    // TODO: Move to repository or something like that :)
    private async Task<Client> FindClientByIdAsync(int clientId, CancellationToken cancellationToken)
    {
        var client = await _db.Clients
            .Include(x => x.AllowedGrantTypes)
            .Include(x => x.RedirectUris)
            .Include(x => x.PostLogoutRedirectUris)
            .Include(x => x.AllowedScopes)
            .Include(x => x.ClientSecrets)
            .Include(x => x.Claims)
            .Include(x => x.IdentityProviderRestrictions)
            .Include(x => x.AllowedCorsOrigins)
            .Include(x => x.Properties)
            .Where(x => x.Id == clientId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        _ = client ?? throw new NotFoundException(_t["Client Not Found."]);

        return client;
    }

    #endregion
}