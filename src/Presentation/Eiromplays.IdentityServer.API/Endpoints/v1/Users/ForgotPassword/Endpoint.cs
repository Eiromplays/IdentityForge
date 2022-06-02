using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ForgotPassword;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/users/forgot-password");
        Summary(s =>
        {
            s.Summary = "Request a password reset email for a user.";
        });
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Message = await _userService.ForgotPasswordAsync(req.Data, BaseURL);

        await SendAsync(Response, cancellation: ct);
    }
}