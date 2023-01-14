using Ardalis.Specification.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Eiromplays.IdentityServer.Application.Common.Events;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Common.Specification;
using Eiromplays.IdentityServer.Application.Identity.IdentityProviders;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using IdentityProvider = Duende.IdentityServer.EntityFramework.Entities.IdentityProvider;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

public class IdentityProviderService : IIdentityProviderService
{
    private readonly ApplicationDbContext _db;
    private readonly IStringLocalizer _t;
    private readonly IEventPublisher _events;

    public IdentityProviderService(ApplicationDbContext db, IStringLocalizer<IdentityProviderService> t, IEventPublisher events)
    {
        _db = db;
        _t = t;
        _events = events;
    }

    public async Task<PaginationResponse<IdentityProviderDto>> SearchAsync(IdentityProviderListFilter filter, CancellationToken cancellationToken = default)
    {
        var spec = new EntitiesByPaginationFilterSpec<IdentityProvider>(filter);

        var identityProviders = await _db.IdentityProviders
            .WithSpecification(spec)
            .Select(x => MapToDto(x))
            .ProjectToType<IdentityProviderDto>()
            .ToListAsync(cancellationToken);

        int count = await _db.IdentityProviders
            .CountAsync(cancellationToken);

        return new PaginationResponse<IdentityProviderDto>(identityProviders, count, filter.PageNumber, filter.PageSize);
    }

    public async Task<IdentityProviderDto> GetAsync(int identityProviderId, CancellationToken cancellationToken = default)
    {
        var client = await _db.IdentityProviders
            .Where(x => x.Id == identityProviderId)
            .Select(x => MapToDto(x))
            .ProjectToType<IdentityProviderDto>()
            .FirstOrDefaultAsync(cancellationToken);

        _ = client ?? throw new NotFoundException(_t["Identity Provider not found"]);

        return client;
    }

    public async Task UpdateAsync(UpdateIdentityProviderRequest request, int identityProviderId,
        CancellationToken cancellationToken = default)
    {
        var identityProvider = await _db.IdentityProviders.FirstOrDefaultAsync(x => x.Id == identityProviderId,
            cancellationToken: cancellationToken);

        var mappedIdentityProvider = MapIdentityProvider(identityProvider);

        _ = mappedIdentityProvider ?? throw new NotFoundException(_t["Identity Provider not found"]);

        mappedIdentityProvider.Scheme = request.Scheme ?? mappedIdentityProvider.Scheme;
        mappedIdentityProvider.DisplayName = request.DisplayName ?? mappedIdentityProvider.DisplayName;
        mappedIdentityProvider.Enabled = request.Enabled ?? mappedIdentityProvider.Enabled;
        mappedIdentityProvider.Type = request.Type ?? mappedIdentityProvider.Type;

        foreach (var property in request.Properties)
        {
            if (mappedIdentityProvider.Properties.ContainsKey(property.Key))
            {
                mappedIdentityProvider.Properties[property.Key] = property.Value;
            }
            else
            {
                mappedIdentityProvider.Properties.Add(property.Key, property.Value);
            }
        }

        _db.IdentityProviders.Update(mappedIdentityProvider.ToEntity());

        bool success = await _db.SaveChangesAsync(cancellationToken) > 0;

        // await _events.PublishAsync(new IdentityProviderUpdatedEvent(identityResource.Id));

        if (!success)
        {
            throw new InternalServerException(
                _t["Update IdentityProvider failed"],
                new List<string> { "Failed to update IdentityResource" });
        }
    }

    public async Task DeleteAsync(int identityProviderId, CancellationToken cancellationToken = default)
    {
        var identityProvider = await _db.IdentityProviders.FirstOrDefaultAsync(
            x => x.Id == identityProviderId,
            cancellationToken: cancellationToken);

        _ = identityProvider ?? throw new NotFoundException(_t["Identity Provider not found"]);

        _db.IdentityProviders.Remove(identityProvider);

        bool success = await _db.SaveChangesAsync(cancellationToken) > 0;

        // await _events.PublishAsync(new IdentityProviderDeletedEvent(identityProvider.Id));

        if (!success)
        {
            throw new InternalServerException(_t["Delete identity provider failed"], new List<string> { "Failed to delete identity provider" });
        }
    }

    public async Task<string> CreateAsync(CreateIdentityProviderRequest request, CancellationToken cancellationToken = default)
    {
        var identityProviderModel = new Duende.IdentityServer.Models.IdentityProvider(request.Type)
        {
            Scheme = request.Scheme,
            DisplayName = request.DisplayName,
            Enabled = request.Enabled,
        };

        foreach (var property in request.Properties)
        {
            if (identityProviderModel.Properties.ContainsKey(property.Key))
            {
                identityProviderModel.Properties[property.Key] = property.Value;
            }
            else
            {
                identityProviderModel.Properties.Add(property.Key, property.Value);
            }
        }

        var identityProvider = identityProviderModel.ToEntity();

        await _db.IdentityProviders.AddAsync(identityProvider, cancellationToken);

        bool success = await _db.SaveChangesAsync(cancellationToken) > 0;

        if (!success) throw new InternalServerException(_t["Create Identity Provider failed"], new List<string> { "Failed to create Provider" });

        // await _events.PublishAsync(new IdentityProviderCreatedEvent(identityResource.Id));

        return string.Format(_t["Identity Provider {0} Registered."], identityProvider.Id);
    }

    public Task<int> GetCountAsync(CancellationToken cancellationToken = default) =>
        _db.IdentityProviders.AsNoTracking().CountAsync(cancellationToken);

    #region Mapping

    private static Duende.IdentityServer.Models.IdentityProvider? MapIdentityProvider(IdentityProvider? identityProvider)
    {
        return identityProvider?.Type.ToLower() switch
        {
            "oidc" => new OidcProvider(identityProvider.ToModel()),
            _ => null
        };
    }

    private static IdentityProviderDto MapToDto(IdentityProvider identityProvider)
    {
        var mappedIdentityProvider = MapIdentityProvider(identityProvider);

        return new IdentityProviderDto
        {
            Id = identityProvider.Id,
            Scheme = identityProvider.Scheme,
            DisplayName = identityProvider.DisplayName,
            Enabled = identityProvider.Enabled,
            Type = identityProvider.Type,
            Properties = mappedIdentityProvider?.Properties ?? new Dictionary<string, string>(),
            Created = identityProvider.Created,
            Updated = identityProvider.Updated,
            LastAccessed = identityProvider.LastAccessed,
            NonEditable = identityProvider.NonEditable
        };
    }

    #endregion
}