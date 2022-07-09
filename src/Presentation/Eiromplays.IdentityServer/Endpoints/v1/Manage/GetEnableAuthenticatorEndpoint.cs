using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class GetEnableAuthenticatorEndpoint : EndpointWithoutRequest<GetEnableAuthenticatorResponse>
{
    private readonly IUserService _userService;

    public GetEnableAuthenticatorEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/manage/two-factor-authentication/enable");
        Summary(s =>
        {
            s.Summary = "Enable two factor authentication";
        });
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _userService.GetEnableTwoFactorAsync(User.GetUserId() ?? string.Empty);

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
                    default:
                        await SendErrorsAsync(cancellation: ct);
                        break;
                }
            });
    }
}