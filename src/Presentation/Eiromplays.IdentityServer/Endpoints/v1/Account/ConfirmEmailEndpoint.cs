using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class ConfirmEmailEndpoint : Endpoint<ConfirmEmailRequest, ConfirmEmailResponse>
{
    private readonly IUserService _userService;
    
    public ConfirmEmailEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/account/confirm-email");
        Summary(s =>
        {
            s.Summary = "Confirm email address for a user.";
        });
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(ConfirmEmailRequest req, CancellationToken ct)
    {
        Response.Message = await _userService.ConfirmEmailAsync(req.UserId, req.Code, ct);
        
        if (!string.IsNullOrWhiteSpace(req.ReturnUrl))
        {
            await SendRedirectAsync(req.ReturnUrl, true, ct);
            return;
        }

        await SendAsync(Response, cancellation: ct);
    }
}