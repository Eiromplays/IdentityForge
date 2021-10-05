using Eiromplays.IdentityServer.Domain.Common;

namespace Eiromplays.IdentityServer.Domain.Events.Permission
{
    public class PermissionCompletedEvent : DomainEvent
    {
        public PermissionCompletedEvent(Entities.Permission permission)
        {
            Permission = permission;
        }

        public Entities.Permission Permission { get; }
    }
}
