using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Shared.Events;

namespace Eiromplays.IdentityServer.Application.Common.Events;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}