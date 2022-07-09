using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Application.Identity.Sessions.Requests;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class DeleteUserSessionEndpoint : Endpoint<DeleteServerSideSessionRequest>
{
    private readonly ISessionManagementService _sessionManagementService;

    public DeleteUserSessionEndpoint(ISessionManagementService sessionManagementService)
    {
        _sessionManagementService = sessionManagementService;
    }

    public override void Configure()
    {
        Delete("/manage/user-sessions/{SessionId}");
        Summary(s =>
        {
            s.Summary = "Delete server-side session by SessionId";
        });
        Version(1);
    }

    public override async Task HandleAsync(DeleteServerSideSessionRequest req, CancellationToken ct)
    {
        await _sessionManagementService.RemoveSessionsAsync(
            new RemoveSessionsContext
        {
            SessionId = req.SessionId,
            RevokeTokens = true
        },
            ct);

        await SendNoContentAsync(ct);
    }
}