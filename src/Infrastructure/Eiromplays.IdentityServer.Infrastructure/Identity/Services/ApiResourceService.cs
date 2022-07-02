using Ardalis.Specification.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.Entities;
using Eiromplays.IdentityServer.Application.Common.Events;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Common.Specification;
using Eiromplays.IdentityServer.Application.Identity.ApiResources;
using Eiromplays.IdentityServer.Domain.Identity;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal class ApiResourceService : IApiResourceService
{
    private readonly ApplicationDbContext _db;
    private readonly IStringLocalizer _t;
    private readonly IEventPublisher _events;

    public ApiResourceService(ApplicationDbContext db, IStringLocalizer<ApiResourceService> t, IEventPublisher events)
    {
        _db = db;
        _t = t;
        _events = events;
    }

    public async Task<PaginationResponse<ApiResourceDto>> SearchAsync(ApiResourceListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<ApiResource>(filter);

        var apiResources = await _db.ApiResources
            .WithSpecification(spec)
            .ProjectToType<ApiResourceDto>()
            .ToListAsync(cancellationToken);

        int count = await _db.ApiResources
            .CountAsync(cancellationToken);

        return new PaginationResponse<ApiResourceDto>(apiResources, count, filter.PageNumber, filter.PageSize);
    }

    public async Task<ApiResourceDto> GetAsync(int apiResourceId, CancellationToken cancellationToken)
    {
        var apiScope = await FindApiResourceByIdAsync(apiResourceId, cancellationToken);

        return apiScope.Adapt<ApiResourceDto>();
    }

    public async Task UpdateAsync(UpdateApiResourceRequest request, int apiResourceId, CancellationToken cancellationToken)
    {
        var apiResource = await FindApiResourceByIdAsync(apiResourceId, cancellationToken);

        apiResource.Name = request.Name;
        apiResource.DisplayName = request.DisplayName;
        apiResource.Description = request.Description;
        apiResource.ShowInDiscoveryDocument = request.ShowInDiscoveryDocument;
        apiResource.AllowedAccessTokenSigningAlgorithms = request.AllowedAccessTokenSigningAlgorithms;
        apiResource.RequireResourceIndicator = request.RequireResourceIndicator;
        apiResource.Enabled = request.Enabled;
        apiResource.NonEditable = request.NonEditable;

        _db.ApiResources.Update(apiResource);

        bool success = await _db.SaveChangesAsync(cancellationToken) > 0;

        await _events.PublishAsync(new ApiResourceUpdatedEvent(apiResource.Id));

        if (!success)
        {
            throw new InternalServerException(
                _t["Update ApiResource failed"],
                new List<string> { "Failed to update ApiResource" });
        }
    }

    public async Task DeleteAsync(int apiResourceId, CancellationToken cancellationToken)
    {
        var apiResource = await FindApiResourceByIdAsync(apiResourceId, cancellationToken);

        _db.ApiResources.Remove(apiResource);

        bool success = await _db.SaveChangesAsync(cancellationToken) > 0;

        await _events.PublishAsync(new ApiResourceDeletedEvent(apiResource.Id));

        if (!success)
        {
            throw new InternalServerException(_t["Delete ApiResource failed"], new List<string> { "Failed to delete ApiResource" });
        }
    }

    public async Task<string> CreateAsync(CreateApiResourceRequest request, CancellationToken cancellationToken)
    {
        var apiResource = new ApiResource
        {
            Name = request.Name,
            DisplayName = request.DisplayName,
            Description = request.Description,
            ShowInDiscoveryDocument = request.ShowInDiscoveryDocument,
            AllowedAccessTokenSigningAlgorithms = request.AllowedAccessTokenSigningAlgorithms,
            RequireResourceIndicator = request.RequireResourceIndicator,
            Enabled = request.Enabled,
            NonEditable = request.NonEditable
        };

        await _db.ApiResources.AddAsync(apiResource, cancellationToken);

        bool success = await _db.SaveChangesAsync(cancellationToken) > 0;

        if (!success) throw new InternalServerException(_t["Create ApiResource failed"], new List<string> { "Failed to create ApiResource" });

        await _events.PublishAsync(new ApiResourceCreatedEvent(apiResource.Id));

        return string.Format(_t["ApiResource {0} Registered."], apiResource.Id);
    }

    #region Entity Queries

    // TODO: Move to repository or something like that :)
    private async Task<ApiResource> FindApiResourceByIdAsync(int apiResourceId, CancellationToken cancellationToken)
    {
        var apiResource = await _db.ApiResources
            .Include(x => x.UserClaims)
            .Include(x => x.Scopes)
            .Where(x => x.Id == apiResourceId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        _ = apiResource ?? throw new NotFoundException(_t["ApiResource Not Found."]);

        return apiResource;
    }

    #endregion
}