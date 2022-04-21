using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.SelfRegister;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/users/self-register");
        Summary(s =>
        {
            s.Summary = "Anonymous user creates a user.";
        });
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        // TODO: Add a option to allow anonymous users to create users
        // Returns Unauthorized if it is disabled
        // TODO: Add some more protection, like a captcha or something
        Response.Message = await _userService.CreateAsync(req.Data, BaseURL);

        await SendAsync(Response, cancellation: ct);
    }
}