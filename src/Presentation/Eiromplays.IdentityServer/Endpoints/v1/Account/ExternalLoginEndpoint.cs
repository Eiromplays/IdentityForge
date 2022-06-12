using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.ExternalLogins;
using Microsoft.AspNetCore.Authentication;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class ExternalLoginEndpoint : Endpoint<ExternalLoginRequest, AuthenticationProperties>
{
    private readonly IAuthService _authService;

    public ExternalLoginEndpoint(IAuthService authService)
    {
        _authService = authService;
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
        var result = await _authService.ExternalLoginAsync<ExternalLoginCallbackEndpoint>(req, HttpContext.Response);

        await result.Match(_ => Task.CompletedTask, exception =>
        {
            if (exception is BadRequestException badRequestException)
            {
                ThrowError(badRequestException.Message);
            }

            return SendErrorsAsync(cancellation: ct);
        });
    }
}