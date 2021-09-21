using System.Threading.Tasks;
using Eiromplays.IdentityServer.Domain.Common;

namespace Eiromplays.IdentityServer.Application.Common.Interface
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
