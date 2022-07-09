using Duende.IdentityServer.Stores;
using Eiromplays.IdentityServer.Application.Identity.Sessions.Requests;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class DeleteUserSessionEndpoint : Endpoint<DeleteServerSideSessionRequest>
{
    private readonly IServerSideSessionStore _serverSideSessionStore;

    public DeleteUserSessionEndpoint(IServerSideSessionStore serverSideSessionStore)
    {
        _serverSideSessionStore = serverSideSessionStore;
    }

    public override void Configure()
    {
        Delete("/manage/user-sessions/{Key}");
        Summary(s =>
        {
            s.Summary = "Delete server-side session by key";
        });
        Version(1);
    }

    public override async Task HandleAsync(DeleteServerSideSessionRequest req, CancellationToken ct)
    {
        await _serverSideSessionStore.DeleteSessionAsync(req.Key, ct);

        await SendNoContentAsync(ct);
    }
}