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

    public GetStatsRequestHandler(IUserService userService, IRoleService roleService)
    {
        _userService = userService;
        _roleService = roleService;
    }

    public async Task<StatsDto> Handle(GetStatsRequest request, CancellationToken cancellationToken)
    {
        var stats = new StatsDto
        {
            UserCount = await _userService.GetCountAsync(cancellationToken),
            RoleCount = await _roleService.GetCountAsync(cancellationToken)
        };

        return stats;
    }
}