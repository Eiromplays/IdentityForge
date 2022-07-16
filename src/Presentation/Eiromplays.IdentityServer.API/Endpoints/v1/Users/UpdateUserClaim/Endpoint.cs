using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.UpdateUserClaim;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Put("/users/{Id}/claims");
        Summary(s =>
        {
            s.Summary = "Update user claim";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Update, EiaResource.UserClaims));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Message = await _userService.UpdateClaimAsync(req.Id, req.UpdateUserClaimRequest);

        await SendOkAsync(Response, cancellation: ct);
    }
}