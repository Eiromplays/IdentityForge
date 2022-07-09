using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Eiromplays.IdentityServer.Application.Identity.Sessions.Requests;
using Org.BouncyCastle.Ocsp;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class GetUserSessionEndpoint : Endpoint<GetServerSideSessionRequest, ServerSideSession>
{
    private readonly IServerSideSessionStore _serverSideSessionStore;

    public GetUserSessionEndpoint(IServerSideSessionStore serverSideSessionStore)
    {
        _serverSideSessionStore = serverSideSessionStore;
    }

    public override void Configure()
    {
        Get("/manage/user-sessions/{Key}");
        Summary(s =>
        {
            s.Summary = "Get server-side session by key";
        });
        Version(1);
    }

    public override async Task HandleAsync(GetServerSideSessionRequest req, CancellationToken ct)
    {
        var userSession = await _serverSideSessionStore.GetSessionAsync(req.Key, ct);

        await SendOkAsync(userSession, ct);
    }
}