using Duende.IdentityServer.Extensions;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.Logins;

namespace Eiromplays.IdentityServer.Endpoints.v1.ExternalLogins;

public class GetExternalLoginsEndpoint : EndpointWithoutRequest<ExternalLoginsResponse>
{
    private readonly IUserService _userService;

    public GetExternalLoginsEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/external-logins");
        Version(1);
        Summary(s =>
        {
            s.Description = "Get external logins";
        });
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetSubjectId();
        
        if (string.IsNullOrWhiteSpace(userId))
            ThrowError("UserId is required");

        Response = await _userService.GetExternalLoginsAsync(userId);

        await SendOkAsync(Response, ct);
    }
}