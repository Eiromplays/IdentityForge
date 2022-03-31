using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ResetPassword;

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
        Routes("/users/forgot-password");
        Summary(s =>
        {
            s.Summary = "Request a password reset email for a user.";
        });
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Message = await _userService.ForgotPasswordAsync(req.ForgotPasswordRequest, BaseURL);

        await SendAsync(Response, cancellation: ct);
    }
}