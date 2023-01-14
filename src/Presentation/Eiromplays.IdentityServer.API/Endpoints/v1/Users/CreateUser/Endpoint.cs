using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.CreateUser;

public class Endpoint : Endpoint<CreateUserRequest, CreateUserResponse>
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
        Policies(EiaPermission.NameFor(EiaAction.Create, EiaResource.Users));
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        // TODO: Add a option to allow anonymous users to create users
        // Returns Unauthorized if it is disabled
        // TODO: Add some more protection, like a captcha or something
        Response = await _userService.CreateAsync(req, BaseURL);

        await SendOkAsync(Response, cancellation: ct);
    }
}