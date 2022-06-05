using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Configuration;
using Eiromplays.IdentityServer.Contracts.v1.Requests.Account;
using Eiromplays.IdentityServer.Contracts.v1.Responses.Account;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class GetLogoutEndpoint : Endpoint<GetLogoutRequest, GetLogoutResponse>
{
    private readonly IIdentityServerInteractionService _interaction;

    public GetLogoutEndpoint(IIdentityServerInteractionService interaction)
    {
        _interaction = interaction;
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
        Response = await BuildLogoutResponseAsync(req.LogoutId);

        if (Response.ShowLogoutPrompt == false)
        {
            // if the request for logout was properly authenticated from IdentityServer, then
            // we don't need to show the prompt and can just log the user out directly.
            return await Logout(Response);
        }

        await SendOkAsync(Response, ct);
    }
    
    private async Task<GetLogoutResponse> BuildLogoutResponseAsync(string logoutId)
    {
        var response = new GetLogoutResponse { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };
            
        if (User.Identity?.IsAuthenticated != true)
        {
            // if the user is not authenticated, then just show logged out page
            response.ShowLogoutPrompt = false;
            return response;
        }

        var context = await _interaction.GetLogoutContextAsync(logoutId);

        // show the logout prompt. this prevents attacks where the user
        // is automatically signed out by another malicious web page.
        if (context?.ShowSignoutPrompt != false)
            return response;

        // it's safe to automatically sign-out
        response.ShowLogoutPrompt = false;
        return response;
    }
}