using Ardalis.Specification.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.Entities;
using Eiromplays.IdentityServer.Application.Common.Events;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Common.Specification;
using Eiromplays.IdentityServer.Application.Identity.IdentityResources;
using Eiromplays.IdentityServer.Domain.Identity;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal class IdentityResourceService : IIdentityResourceService
{
    private readonly ApplicationDbContext _db;
    private readonly IStringLocalizer _t;
    private readonly IEventPublisher _events;

    public IdentityResourceService(ApplicationDbContext db, IStringLocalizer<IdentityResourceService> t, IEventPublisher events)
    {
        _db = db;
        _t = t;
        _events = events;
    }

    public async Task<PaginationResponse<IdentityResourceDto>> SearchAsync(IdentityResourceListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<IdentityResource>(filter);

        var identityResources = await _db.IdentityResources
            .WithSpecification(spec)
            .ProjectToType<IdentityResourceDto>()
            .ToListAsync(cancellationToken);

        int count = await _db.IdentityResources
            .CountAsync(cancellationToken);

        return new PaginationResponse<IdentityResourceDto>(identityResources, count, filter.PageNumber, filter.PageSize);
    }

    public async Task<IdentityResourceDto> GetAsync(int identityResourceId, CancellationToken cancellationToken)
    {
        var client = await FindIdentityResourceByIdAsync(identityResourceId, cancellationToken);

        return client.Adapt<IdentityResourceDto>();
    }

    public async Task UpdateAsync(UpdateIdentityResourceRequest request, int identityResourceId, CancellationToken cancellationToken)
    {
        var identityResource = await FindIdentityResourceByIdAsync(identityResourceId, cancellationToken);

        identityResource.Name = request.Name;
        identityResource.DisplayName = request.DisplayName;
        identityResource.Description = request.Description;
        identityResource.ShowInDiscoveryDocument = request.ShowInDiscoveryDocument;
        identityResource.Emphasize = request.Emphasize;
        identityResource.Required = request.Required;
        identityResource.Enabled = request.Enabled;
        identityResource.NonEditable = request.NonEditable;

        _db.IdentityResources.Update(identityResource);

        bool success = await _db.SaveChangesAsync(cancellationToken) > 0;

        await _events.PublishAsync(new IdentityResourceUpdatedEvent(identityResource.Id));

        if (!success)
        {
            throw new InternalServerException(
                _t["Update IdentityResource failed"],
                new List<string> { "Failed to update IdentityResource" });
        }
    }

    public async Task DeleteAsync(int identityResourceId, CancellationToken cancellationToken)
    {
        var identityResource = await FindIdentityResourceByIdAsync(identityResourceId, cancellationToken);

        _db.IdentityResources.Remove(identityResource);

        bool success = await _db.SaveChangesAsync(cancellationToken) > 0;

        await _events.PublishAsync(new IdentityResourceDeletedEvent(identityResource.Id));

        if (!success)
        {
            throw new InternalServerException(_t["Delete client failed"], new List<string> { "Failed to delete client" });
        }
    }

    public async Task<string> CreateAsync(CreateIdentityResourceRequest request, CancellationToken cancellationToken)
    {
        var identityResource = new IdentityResource
        {
            Name = request.Name,
            DisplayName = request.DisplayName,
            Description = request.Description,
            ShowInDiscoveryDocument = request.ShowInDiscoveryDocument,
            Emphasize = request.Emphasize,
            Required = request.Required,
            Enabled = request.Enabled,
            NonEditable = request.NonEditable
        };

        await _db.IdentityResources.AddAsync(identityResource, cancellationToken);

        bool success = await _db.SaveChangesAsync(cancellationToken) > 0;

        if (!success) throw new InternalServerException(_t["Create IdentityResource failed"], new List<string> { "Failed to create IdentityResource" });

        await _events.PublishAsync(new IdentityResourceCreatedEvent(identityResource.Id));

        return string.Format(_t["IdentityResource {0} Registered."], identityResource.Id);
    }

    #region Entity Queries

    // TODO: Move to repository or something like that :)
    private async Task<IdentityResource> FindIdentityResourceByIdAsync(int identityResourceId, CancellationToken cancellationToken)
    {
        var client = await _db.IdentityResources
            .Include(x => x.UserClaims)
            .Where(x => x.Id == identityResourceId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        _ = client ?? throw new NotFoundException(_t["IdentityResource Not Found."]);

        return client;
    }

    #endregion
}