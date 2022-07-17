using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.Claims;

public class AddUserClaimEndpoint : Endpoint<AddUserClaimModels.Request, AddUserClaimModels.Response>
{
    private readonly IUserService _userService;

    public AddUserClaimEndpoint(IUserService userService)
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

    public override async Task HandleAsync(AddUserClaimModels.Request req, CancellationToken ct)
    {
        Response.Message = await _userService.AddClaimAsync(req.Id, req.AddUserClaimRequest);

        await SendOkAsync(Response, cancellation: ct);
    }
}