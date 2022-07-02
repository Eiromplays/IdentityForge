using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.Search;

public class Endpoint : Endpoint<UserListFilter, PaginationResponse<UserDetailsDto>>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/users/search");
        Summary(s =>
        {
            s.Summary = "Search users using available filters.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Search, EiaResource.Users));
    }

    public override async Task HandleAsync(UserListFilter request, CancellationToken ct)
    {
        Response = await _userService.SearchAsync(request, ct);

        await SendAsync(Response, cancellation: ct);
    }
}