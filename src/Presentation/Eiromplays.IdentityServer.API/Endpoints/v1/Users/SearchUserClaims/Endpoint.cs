using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.Claims;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.SearchUserClaims;

public class Endpoint : Endpoint<UserClaimListFilter, PaginationResponse<UserClaimDto>>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/users/{Id}/claims-search");
        Summary(s =>
        {
            s.Summary = "Search for user claims";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Search, EiaResource.UserClaims));
    }

    public override async Task HandleAsync(UserClaimListFilter request, CancellationToken ct)
    {
        Response = await _userService.SearchUserClaimsAsync(request, ct);

        await SendAsync(Response, cancellation: ct);
    }
}