using Duende.IdentityServer.EntityFramework.Mappers;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
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