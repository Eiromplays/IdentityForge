using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Domain.Events.Permission;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Application.Permissions.EventHandlers
{
    public class PermissionCreatedEventHandler : INotificationHandler<DomainEventNotification<PermissionCreatedEvent>>
    {
        private readonly ILogger<PermissionCreatedEventHandler> _logger;

        public PermissionCreatedEventHandler(ILogger<PermissionCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<PermissionCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Eiromplays.IdentityServer Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
