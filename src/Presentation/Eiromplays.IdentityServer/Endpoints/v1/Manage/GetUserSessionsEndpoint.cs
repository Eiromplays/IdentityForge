using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;

namespace Eiromplays.IdentityServer.Endpoints.v1.Manage;

public class GetUserSessionsEndpoint : EndpointWithoutRequest<QueryResult<ServerSideSession>>
{
    private readonly IServerSideSessionStore _serverSideSessionStore;

    public GetUserSessionsEndpoint(IServerSideSessionStore serverSideSessionStore)
    {
        _serverSideSessionStore = serverSideSessionStore;
    }

    public override void Configure()
    {
        Get("/manage/user-sessions");
        Summary(s =>
        {
            s.Summary = "Get all server-side sessions for the current user";
        });
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userSessions =
            await _serverSideSessionStore.QuerySessionsAsync(new SessionQuery { SubjectId = User.GetUserId() }, ct);

        await SendOkAsync(userSessions, ct);
    }
}