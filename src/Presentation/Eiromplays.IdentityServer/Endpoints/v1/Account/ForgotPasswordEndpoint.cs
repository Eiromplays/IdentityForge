using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.Password;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class ForgotPasswordEndpoint : Endpoint<ForgotPasswordRequest, ForgotPasswordResponse>
{
    private readonly IUserService _userService;

    public ForgotPasswordEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/account/forgot-password");
        Summary(s =>
        {
            s.Summary = "Request a password reset email for a user.";
        });
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(ForgotPasswordRequest req, CancellationToken ct)
    {
        Response.Message = await _userService.ForgotPasswordAsync(req, BaseURL);

        await SendAsync(Response, cancellation: ct);
    }
}