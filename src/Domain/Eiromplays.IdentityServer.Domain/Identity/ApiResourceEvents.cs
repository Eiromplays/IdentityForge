namespace Eiromplays.IdentityServer.Domain.Identity;

public abstract class ApiResourceEvent : DomainEvent
{
    public int ApiResourceId { get; set; }

    protected ApiResourceEvent(int apiResourceId) => ApiResourceId = apiResourceId;
}

public class ApiResourceCreatedEvent : ApiResourceEvent
{
    public ApiResourceCreatedEvent(int apiResourceId)
        : base(apiResourceId)
    {
    }
}

public class ApiResourceUpdatedEvent : ApiResourceEvent
{

    public ApiResourceUpdatedEvent(int apiResourceId)
        : base(apiResourceId)
    {
        
    }
}

public class ApiResourceDeletedEvent : ApiResourceEvent
{
    public ApiResourceDeletedEvent(int apiResourceId)
        : base(apiResourceId)
    {
        
    }
}