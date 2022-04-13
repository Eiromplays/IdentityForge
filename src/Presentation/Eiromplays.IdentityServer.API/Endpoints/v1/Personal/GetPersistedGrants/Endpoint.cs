using Eiromplays.IdentityServer.Application.Identity.PersistedGrants;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.GetPersistedGrants;

public class Endpoint : EndpointWithoutRequest<List<PersistedGrantDto>>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/personal/persisted-grants");
        Summary(s =>
        {
            s.Summary = "Get persisted grants of currently logged in user.";
        });
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        //Response = await _userService.Getp(userId, ct);
        
        await SendOkAsync(Response, cancellation: ct);
    }
}