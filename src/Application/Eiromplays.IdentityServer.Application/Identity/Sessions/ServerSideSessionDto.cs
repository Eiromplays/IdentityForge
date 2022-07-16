namespace Eiromplays.IdentityServer.Application.Identity.Sessions;

public class ServerSideSessionDto
{
    public string Key { get; set; } = default!;

    public string Scheme { get; set; } = default!;

    public string SubjectId { get; set; } = default!;

    public string SessionId { get; set; } = default!;

    public string DisplayName { get; set; } = default!;

    public DateTime Created { get; set; }

    public DateTime Renewed { get; set; }

    public DateTime? Expires { get; set; }
}