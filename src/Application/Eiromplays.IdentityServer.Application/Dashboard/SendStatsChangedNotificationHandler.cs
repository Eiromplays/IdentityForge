using Eiromplays.IdentityServer.Domain.Identity;

namespace Eiromplays.IdentityServer.Application.Dashboard;

public class SendStatsChangedNotificationHandler :
    IEventNotificationHandler<ApplicationRoleCreatedEvent>,
    IEventNotificationHandler<ApplicationRoleDeletedEvent>,
    IEventNotificationHandler<ApplicationUserCreatedEvent>,
    IEventNotificationHandler<ApplicationUserDeletedEvent>,
    IEventNotificationHandler<ClientCreatedEvent>,
    IEventNotificationHandler<ClientDeletedEvent>,
    IEventNotificationHandler<ClientUpdatedEvent>,
    IEventNotificationHandler<IdentityResourceCreatedEvent>,
    IEventNotificationHandler<IdentityResourceDeletedEvent>,
    IEventNotificationHandler<IdentityResourceUpdatedEvent>,
    IEventNotificationHandler<ApiResourceCreatedEvent>,
    IEventNotificationHandler<ApiResourceDeletedEvent>,
    IEventNotificationHandler<ApiScopeCreatedEvent>,
    IEventNotificationHandler<ApiResourceUpdatedEvent>,
    IEventNotificationHandler<ApiScopeDeletedEvent>,
    IEventNotificationHandler<ApiScopeUpdatedEvent>
{
    private readonly ILogger<SendStatsChangedNotificationHandler> _logger;
    private readonly INotificationSender _notifications;

    public SendStatsChangedNotificationHandler(ILogger<SendStatsChangedNotificationHandler> logger, INotificationSender notifications) =>
        (_logger, _notifications) = (logger, notifications);

    public Task Handle(EventNotification<ApplicationRoleCreatedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);
    public Task Handle(EventNotification<ApplicationRoleDeletedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);
    public Task Handle(EventNotification<ApplicationUserCreatedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);
    public Task Handle(EventNotification<ClientCreatedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);

    public Task Handle(EventNotification<IdentityResourceCreatedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);

    public Task Handle(EventNotification<ApiResourceCreatedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);

    public Task Handle(EventNotification<ApiScopeCreatedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);

    public Task Handle(EventNotification<ApplicationUserDeletedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);

    public Task Handle(EventNotification<ClientDeletedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);

    public Task Handle(EventNotification<IdentityResourceDeletedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);

    public Task Handle(EventNotification<ApiResourceDeletedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);

    public Task Handle(EventNotification<ApiScopeDeletedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);
    public Task Handle(EventNotification<ClientUpdatedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);

    public Task Handle(EventNotification<IdentityResourceUpdatedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);

    public Task Handle(EventNotification<ApiResourceUpdatedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);

    public Task Handle(EventNotification<ApiScopeUpdatedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);

    private Task SendStatsChangedNotification(IEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Event} Triggered => Sending StatsChangedNotification", @event.GetType().Name);

        return _notifications.SendToAllAsync(new StatsChangedNotification(), cancellationToken);
    }
}