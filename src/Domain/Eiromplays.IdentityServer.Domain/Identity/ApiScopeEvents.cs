namespace Eiromplays.IdentityServer.Domain.Identity;

public abstract class ApiScopeEvent : DomainEvent
{
    public int ApiScopeId { get; set; }

    protected ApiScopeEvent(int apiScopeId) => ApiScopeId = apiScopeId;
}

public class ApiScopeCreatedEvent : ApiScopeEvent
{
    public ApiScopeCreatedEvent(int apiScopeId)
        : base(apiScopeId)
    {
    }
}

public class ApiScopeUpdatedEvent : ApiScopeEvent
{
    public ApiScopeUpdatedEvent(int apiScopeId)
        : base(apiScopeId)
    {
    }
}

public class ApiScopeDeletedEvent : ApiScopeEvent
{
    public ApiScopeDeletedEvent(int apiScopeId)
        : base(apiScopeId)
    {
    }
}