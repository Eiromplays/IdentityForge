using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class EnableAuthenticatorEndpoint : Endpoint<EnableAuthenticatorRequest, EnableAuthenticatorResponse>
{
    private readonly IUserService _userService;

    public EnableAuthenticatorEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/manage/two-factor-authentication/enable");
        Summary(s =>
        {
            s.Summary = "Enable two factor authentication";
        });
        Version(1);
        ScopedValidator();
    }

    public override async Task HandleAsync(EnableAuthenticatorRequest req, CancellationToken ct)
    {
        var result = await _userService.EnableTwoFactorAsync(req, User);

        await result.Match(
            async x =>
            {
                await SendOkAsync(x, cancellation: ct);
            },
            async exception =>
            {
                switch (exception)
                {
                    case NotFoundException notFoundException:
                        AddError(notFoundException.Message);
                        await SendErrorsAsync((int)notFoundException.StatusCode, ct);
                        return;
                    case BadRequestException badRequestException:
                        ThrowError(badRequestException.Message);
                        return;
                    default:
                        await SendErrorsAsync(cancellation: ct);
                        break;
                }
            });
    }
}