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
        Verbs(Http.POST);
        Routes("/users/reset-password");
        Summary(s =>
        {
            s.Summary = "Reset a user's password.";
        });
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Message = await _userService.ResetPasswordAsync(req.ResetPasswordRequest);

        await SendAsync(Response, cancellation: ct);
    }
}