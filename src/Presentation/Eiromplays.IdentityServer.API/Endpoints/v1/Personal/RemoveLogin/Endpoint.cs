using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.RemoveLogin;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/personal/remove-external-login");
        Summary(s =>
        {
            s.Summary = "Remove login from currently logged in user.";
        });
        Version(1);
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        
        Response.Message = await _userService.RemoveLoginAsync(req.Data, userId);
        
        await SendAsync(Response, cancellation: ct);
    }
}