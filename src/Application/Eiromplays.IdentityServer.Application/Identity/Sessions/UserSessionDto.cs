namespace Eiromplays.IdentityServer.Application.Identity.Sessions;

public class UserSessionDto
{
    public long Id { get; set; }

    public string Key { get; set; } = default!;

    public string SubjectId { get; set; } = default!;

    public string? SessionId { get; set; }

    public string ApplicationName { get; set; } = default!;

    public DateTime Created { get; set; }

    public DateTime Renewed { get; set; }

    public DateTime? Expires { get; set; }
}