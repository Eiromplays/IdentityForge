using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.CreateUser;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/users");
        Summary(s =>
        {
            s.Summary = "Creates a new user.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Create, EIAResource.Users));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        // TODO: Add a option to allow anonymous users to create users
        // Returns Unauthorized if it is disabled
        // TODO: Add some more protection, like a captcha or something
        Response.UserId = await _userService.CreateAsync(req.Data, BaseURL);

        await SendCreatedAtAsync<GetUserById.Endpoint>(new GetUserById.Models.Request { Id = Response.UserId },
            Response, cancellation: ct);
    }
}