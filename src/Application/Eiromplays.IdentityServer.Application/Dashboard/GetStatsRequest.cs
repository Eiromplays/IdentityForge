using Eiromplays.IdentityServer.Application.Identity.ApiResources;
using Eiromplays.IdentityServer.Application.Identity.ApiScopes;
using Eiromplays.IdentityServer.Application.Identity.Clients;
using Eiromplays.IdentityServer.Application.Identity.IdentityResources;
using Eiromplays.IdentityServer.Application.Identity.Roles;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.Application.Dashboard;

public class GetStatsRequest : IRequest<StatsDto>
{
}

public class GetStatsRequestHandler : IRequestHandler<GetStatsRequest, StatsDto>
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly IClientService _clientService;
    private readonly IApiResourceService _apiResourceService;
    private readonly IApiScopeService _apiScopeService;
    private readonly IIdentityResourceService _identityResourceService;

    public GetStatsRequestHandler(
        IUserService userService,
        IRoleService roleService,
        IClientService clientService,
        IApiResourceService apiResourceService,
        IApiScopeService apiScopeService, IIdentityResourceService identityResourceService)
    {
        _userService = userService;
        _roleService = roleService;
        _clientService = clientService;
        _apiResourceService = apiResourceService;
        _apiScopeService = apiScopeService;
        _identityResourceService = identityResourceService;
    }

    public async Task<StatsDto> Handle(GetStatsRequest request, CancellationToken cancellationToken)
    {
        var stats = new StatsDto
        {
            UserCount = await _userService.GetCountAsync(cancellationToken),
            RoleCount = await _roleService.GetCountAsync(cancellationToken),
            ClientCount = await _clientService.GetCountAsync(cancellationToken),
            ApiResourceCount = await _apiResourceService.GetCountAsync(cancellationToken),
            ApiScopeCount = await _apiScopeService.GetCountAsync(cancellationToken),
            IdentityResourceCount = await _identityResourceService.GetCountAsync(cancellationToken)
        };

        return stats;
    }
}