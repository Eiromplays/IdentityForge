using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.DeleteUser;

public class Endpoint : Endpoint<Models.Request>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Delete("/users/{Id}");
        Summary(s =>
        {
            s.Summary = "Delete a user.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Delete, EIAResource.Users));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        await _userService.DeleteAsync(req.Id);

        await SendNoContentAsync(cancellation: ct);
    }
}