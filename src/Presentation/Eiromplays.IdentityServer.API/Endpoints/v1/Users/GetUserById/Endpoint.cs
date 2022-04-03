using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.GetUserById;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/users/{Id}");
        Summary(s =>
        {
            s.Summary = "Get a user's details";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.View, EIAResource.Users));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.UserDetails = await _userService.GetAsync(req.Id, ct);

        await SendAsync(Response, cancellation: ct);
    }
}