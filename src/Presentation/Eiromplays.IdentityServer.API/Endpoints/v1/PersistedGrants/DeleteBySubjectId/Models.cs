namespace Eiromplays.IdentityServer.API.Endpoints.v1.PersistedGrants.DeleteBySubjectId;

public class Models
{
    public class Request
    {
        public string SubjectId { get; set; } = default!;
    }

    public class Response
    {
        public string Message { get; set; } = default!;
    }
}