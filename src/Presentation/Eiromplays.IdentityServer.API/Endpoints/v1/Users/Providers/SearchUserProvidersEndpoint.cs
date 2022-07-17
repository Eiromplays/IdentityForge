using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.Logins;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.Providers;

public class SearchUserProvidersEndpoint : Endpoint<UserProviderListFilter, PaginationResponse<UserLoginInfoDto>>
{
    private readonly IUserService _userService;

    public SearchUserProvidersEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/users/{UserId}/providers-search", "/users/providers-search");
        Summary(s =>
        {
            s.Summary = "Search for user providers";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Search, EiaResource.UserClaims));
    }

    public override async Task HandleAsync(UserProviderListFilter request, CancellationToken ct)
    {
        Response = await _userService.SearchUserProvidersAsync(request, ct);

        await SendAsync(Response, cancellation: ct);
    }
}