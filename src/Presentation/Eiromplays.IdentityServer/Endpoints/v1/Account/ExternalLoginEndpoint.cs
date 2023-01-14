using System.Text.Json;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.ExternalLogins;
using Microsoft.AspNetCore.Authentication;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class ExternalLoginEndpoint : Endpoint<ExternalLoginRequest, AuthenticationProperties>
{
    private readonly IAuthService _authService;
    private readonly ILogger<ExternalLoginEndpoint> _logger;

    public ExternalLoginEndpoint(IAuthService authService, ILogger<ExternalLoginEndpoint> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public override void Configure()
    {
        Verbs(Http.GET, Http.POST);
        Routes("/account/external-logins");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Get or create an external login";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(ExternalLoginRequest req, CancellationToken ct)
    {
        var authenticationProperties = await _authService.ExternalLoginAsync<ExternalLoginCallbackEndpoint>(req, HttpContext.Response);

        if (HttpContext.Response.HasStarted)
        {
            _logger.LogWarning("The response has already started, the external login middleware will not be able to redirect the browser");
            return;
        }

        await SendOkAsync(
            authenticationProperties, ct);
    }
}