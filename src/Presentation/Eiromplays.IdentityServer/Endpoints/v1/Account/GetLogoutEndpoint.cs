using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;
using Eiromplays.IdentityServer.Configuration;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class GetLogoutEndpoint : Endpoint<GetLogoutRequest, GetLogoutResponse>
{
    private readonly IAuthService _authService;

    public GetLogoutEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Get("/account/logout");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Get logout information";
        });
    }
    
    public override async Task HandleAsync(GetLogoutRequest req, CancellationToken ct)
    {
        // Build a response so the logout page knows what to display
        Response = await _authService.BuildLogoutResponseAsync(req.LogoutId, AccountOptions.ShowLogoutPrompt);

        if (Response.ShowLogoutPrompt == false)
        {
            // if the request for logout was properly authenticated from IdentityServer, then
            // we don't need to show the prompt and can just log the user out directly.
            await _authService.LogoutAsync<GetLogoutEndpoint>(new LogoutRequest{ LogoutId = Response.LogoutId }, HttpContext);
            return;
        }

        await SendOkAsync(Response, ct);
    }
}