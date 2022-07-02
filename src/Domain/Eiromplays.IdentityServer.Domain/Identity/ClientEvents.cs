namespace Eiromplays.IdentityServer.Domain.Identity;

public abstract class ClientEvent : DomainEvent
{
    public int ClientId { get; set; }

    protected ClientEvent(int clientId) => ClientId = clientId;
}

public class ClientCreatedEvent : ClientEvent
{
    public ClientCreatedEvent(int clientId)
        : base(clientId)
    {
    }
}

public class ClientUpdatedEvent : ClientEvent
{
    public ClientUpdatedEvent(int clientId)
        : base(clientId)
    {
    }
}

public class ClientDeletedEvent : ClientEvent
{
    public ClientDeletedEvent(int clientId)
        : base(clientId)
    {
    }
}