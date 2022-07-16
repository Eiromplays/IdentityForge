using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.Claims;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.AddUserClaim;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/users/{Id}/claims");
        Summary(s =>
        {
            s.Summary = "Add a claim to a user";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Create, EiaResource.UserClaims));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Message = await _userService.AddClaimAsync(req.Id, req.AddUserClaimRequest);

        await SendOkAsync(Response, cancellation: ct);
    }
}