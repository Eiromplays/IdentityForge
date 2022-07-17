using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.Claims;

public class UpdateUserClaimEndpoint : Endpoint<UpdateUserClaimModels.Request, UpdateUserClaimModels.Response>
{
    private readonly IUserService _userService;

    public UpdateUserClaimEndpoint(IUserService userService)
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

    public override async Task HandleAsync(UpdateUserClaimModels.Request req, CancellationToken ct)
    {
        Response.Message = await _userService.UpdateClaimAsync(req.Id, req.UpdateUserClaimRequest);

        await SendOkAsync(Response, cancellation: ct);
    }
}