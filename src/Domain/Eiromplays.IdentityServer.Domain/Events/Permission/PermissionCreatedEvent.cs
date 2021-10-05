using Eiromplays.IdentityServer.Domain.Common;

namespace Eiromplays.IdentityServer.Domain.Events.Permission
{
    public class PermissionCreatedEvent : DomainEvent
    {
        public PermissionCreatedEvent(Entities.Permission permission)
        {
            Permission = permission;
        }

        public Entities.Permission Permission { get;}
    }
}