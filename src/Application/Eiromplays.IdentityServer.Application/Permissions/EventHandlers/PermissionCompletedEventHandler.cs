using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Domain.Events.Permission;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Application.Permissions.EventHandlers
{
    public class PermissionCompletedEventHandler : INotificationHandler<DomainEventNotification<PermissionCompletedEvent>>
    {
        private readonly ILogger<PermissionCompletedEventHandler> _logger;

        public PermissionCompletedEventHandler(ILogger<PermissionCompletedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<PermissionCompletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Eiromplays.IdentityServer Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
