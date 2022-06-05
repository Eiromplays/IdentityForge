
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Eiromplays.IdentityServer.Application.Identity.Sessions;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.GetUserSessions;

public class Endpoint : EndpointWithoutRequest<List<UserSessionDto>>
{
    private readonly IUserService _userService;
    private readonly ISessionManagementService _sessionManagementService;
    
    public Endpoint(IUserService userService, ISessionManagementService sessionManagementService)
    {
        _userService = userService;
        _sessionManagementService = sessionManagementService;
    }

    public override void Configure()
    {
        Get("/personal/user-sessions");
        Summary(s =>
        {
            s.Summary = "Get user sessions of currently logged in user.";
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
        
        Response = await _userService.GetUserSessionsAsync(userId, ct);
        
        await SendAsync(Response, cancellation: ct);
    }
}