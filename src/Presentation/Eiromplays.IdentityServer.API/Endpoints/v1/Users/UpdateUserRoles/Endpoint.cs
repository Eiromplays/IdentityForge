using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.UpdateUserRoles;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/users/{Id}/roles");
        Summary(s =>
        {
            s.Summary = "Update a user's assigned roles.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Update, EiaResource.UserRoles));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Message = await _userService.AssignRolesAsync(req.Id, req.UserRolesRequest, ct);

        await SendAsync(Response, cancellation: ct);
    }
}