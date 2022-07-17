using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.Claims;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.Claims;

public class SearchUserClaimsEndpoint : Endpoint<UserClaimListFilter, PaginationResponse<UserClaimDto>>
{
    private readonly IUserService _userService;

    public SearchUserClaimsEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/users/{UserId}/claims-search", "/users/claims-search");
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