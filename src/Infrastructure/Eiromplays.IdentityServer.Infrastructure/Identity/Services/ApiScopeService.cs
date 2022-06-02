using Ardalis.Specification.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.Entities;
using Eiromplays.IdentityServer.Application.Common.Events;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Common.Specification;
using Eiromplays.IdentityServer.Application.Identity.ApiScopes;
using Eiromplays.IdentityServer.Application.Identity.IdentityResources;
using Eiromplays.IdentityServer.Domain.Identity;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class  ApiScopeService : IApiScopeService
{
    private readonly ApplicationDbContext _db;
    private readonly IStringLocalizer _t;
    private readonly ILogger _logger;
    private readonly IEventPublisher _events;
    
    public ApiScopeService(ApplicationDbContext db, IStringLocalizer<ApiScopeService> t,
        ILogger<ApiScopeService> logger,
        IEventPublisher events)
    {
        _db = db;
        _t = t;
        _logger = logger;
        _events = events;
    }
    
    public async Task<PaginationResponse<ApiScopeDto>> SearchAsync(ApiScopeListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<ApiScope>(filter);

        var apiScopes = await _db.ApiScopes
            .WithSpecification(spec)
            .ProjectToType<ApiScopeDto>()
            .ToListAsync(cancellationToken);

        var count = await _db.ApiScopes
            .CountAsync(cancellationToken);

        return new PaginationResponse<ApiScopeDto>(apiScopes, count, filter.PageNumber, filter.PageSize);
    }

    public async Task<ApiScopeDto> GetAsync(int apiScopeId, CancellationToken cancellationToken)
    {
        var apiScope = await FindApiScopeByIdAsync(apiScopeId, cancellationToken);
        
        return apiScope.Adapt<ApiScopeDto>();
    }
    
    public async Task UpdateAsync(UpdateApiScopeRequest request, int apiScopeId, CancellationToken cancellationToken)
    {
        var apiScope = await FindApiScopeByIdAsync(apiScopeId, cancellationToken);

        apiScope.Name = request.Name;
        apiScope.DisplayName = request.DisplayName;
        apiScope.Description = request.Description;
        apiScope.ShowInDiscoveryDocument = request.ShowInDiscoveryDocument;
        apiScope.Emphasize = request.Emphasize;
        apiScope.Required = request.Required;
        apiScope.Enabled = request.Enabled;
        apiScope.NonEditable = request.NonEditable;
        
        _db.ApiScopes.Update(apiScope);

        var success = await _db.SaveChangesAsync(cancellationToken) > 0;

        await _events.PublishAsync(new ApiScopeUpdatedEvent(apiScope.Id));
        
        if (!success)
        {
            throw new InternalServerException(_t["Update ApiScope failed"],
                new List<string> { "Failed to update ApiScope" });
        }
    }
    
    public async Task DeleteAsync(int apiScopeId, CancellationToken cancellationToken)
    {
        var apiScope = await FindApiScopeByIdAsync(apiScopeId, cancellationToken);

        _db.ApiScopes.Remove(apiScope);

        var success = await _db.SaveChangesAsync(cancellationToken) > 0;

        await _events.PublishAsync(new ApiScopeDeletedEvent(apiScope.Id));
        
        if (!success)
        {
            throw new InternalServerException(_t["Delete ApiScope failed"], new List<string>{ "Failed to delete ApiScope" });
        }
    }
    
    public async Task<string> CreateAsync(CreateApiScopeRequest request, CancellationToken cancellationToken)
    {
        var apiScope = new ApiScope
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

        await _db.ApiScopes.AddAsync(apiScope, cancellationToken);

        var success = await _db.SaveChangesAsync(cancellationToken) > 0;
        
        if (!success) throw new InternalServerException(_t["Create ApiScope failed"], new List<string>{ "Failed to create ApiScope" });
        
        await _events.PublishAsync(new ApiScopeCreatedEvent(apiScope.Id));

        return string.Format(_t["ApiScope {0} Registered."], apiScope.Id);
    }
    
    #region Entity Queries

    // TODO: Move to repository or something like that :)
    private async Task<ApiScope> FindApiScopeByIdAsync(int apiScopeId, CancellationToken cancellationToken)
    {
        var apiScope = await _db.ApiScopes
            .Include(x => x.UserClaims)
            .Include(x=> x.Properties)
            .Where(x => x.Id == apiScopeId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        _ = apiScope ?? throw new NotFoundException(_t["ApiScope Not Found."]);

        return apiScope;
    }

    #endregion
}