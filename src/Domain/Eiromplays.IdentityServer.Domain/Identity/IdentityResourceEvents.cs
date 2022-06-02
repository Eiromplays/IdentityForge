namespace Eiromplays.IdentityServer.Domain.Identity;

public abstract class IdentityResourceEvent : DomainEvent
{
    public int IdentityResourceId { get; set; }

    protected IdentityResourceEvent(int identityResourceId) => IdentityResourceId = identityResourceId;
}

public class IdentityResourceCreatedEvent : IdentityResourceEvent
{
    public IdentityResourceCreatedEvent(int identityResourceId)
        : base(identityResourceId)
    {
    }
}

public class IdentityResourceUpdatedEvent : IdentityResourceEvent
{

    public IdentityResourceUpdatedEvent(int identityResourceId)
        : base(identityResourceId)
    {
        
    }
}

public class IdentityResourceDeletedEvent : IdentityResourceEvent
{
    public IdentityResourceDeletedEvent(int identityResourceId)
        : base(identityResourceId)
    {
        
    }
}