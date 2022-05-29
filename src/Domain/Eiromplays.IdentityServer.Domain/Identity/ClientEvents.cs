namespace Eiromplays.IdentityServer.Domain.Identity;

public abstract class ClientEvent : DomainEvent
{
    public string ClientId { get; set; } = default!;

    protected ClientEvent(string clientId) => ClientId = clientId;
}

public class ClientCreatedEvent : ClientEvent
{
    public ClientCreatedEvent(string clientId)
        : base(clientId)
    {
    }
}

public class ClientUpdatedEvent : ClientEvent
{

    public ClientUpdatedEvent(string clientId, bool rolesUpdated = false)
        : base(clientId)
    {
        
    }
}

public class ClientDeletedEvent : ClientEvent
{
    public ClientDeletedEvent(string clientId)
        : base(clientId)
    {
        
    }
}