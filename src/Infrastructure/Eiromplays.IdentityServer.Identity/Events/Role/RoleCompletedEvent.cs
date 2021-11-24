using Eiromplays.IdentityServer.Domain.Common;
using Eiromplays.IdentityServer.Identity.DTOs;

namespace Eiromplays.IdentityServer.Identity.Events.Role
{
    public class RoleCompletedEvent : DomainEvent
    {
        public RoleCompletedEvent(RoleDto roleDto)
        {
            RoleDto = roleDto;
        }

        public RoleDto RoleDto { get; }
    }
}
