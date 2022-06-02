using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.UpdateUser;

public class Endpoint : Endpoint<Models.Request>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Put("/users/{Id}");
        Summary(s =>
        {
            s.Summary = "Update a user.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Update, EIAResource.Users));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        await _userService.UpdateAsync(req.Data, req.Id, ct);
        
        await SendNoContentAsync(cancellation: ct);
    }
}