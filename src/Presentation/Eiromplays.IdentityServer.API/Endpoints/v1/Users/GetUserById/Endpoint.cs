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
        Verbs(Http.GET);
        Routes("/users/{Id}");
        Summary(s =>
        {
            s.Summary = "Get a user's details";
        });
        Version(1);
        Policies("RequireAdministrator");
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        var user = await _userService.GetAsync(req.Id, ct);

        await SendAsync(new Models.Response{ UserDetailsDto = user }, cancellation: ct);
    }
}