using Eiromplays.IdentityServer.Domain.Common;

namespace Eiromplays.IdentityServer.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
