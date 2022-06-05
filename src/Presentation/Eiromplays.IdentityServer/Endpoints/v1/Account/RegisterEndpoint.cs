using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Contracts.v1.Requests.Account;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class RegisterEndpoint : Endpoint<RegisterRequest, CreateUserResponse>
{
    private readonly IUserService _userService;
    
    public RegisterEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/account/register");
        Summary(s =>
        {
            s.Summary = "Register a new user";
        });
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        // TODO: Add a option to allow anonymous users to create users
        // Returns Unauthorized if it is disabled
        // TODO: Add some more protection, like a captcha or something
        Response = await _userService.CreateAsync(req.Data, BaseURL);

        await SendAsync(Response, cancellation: ct);
    }
}