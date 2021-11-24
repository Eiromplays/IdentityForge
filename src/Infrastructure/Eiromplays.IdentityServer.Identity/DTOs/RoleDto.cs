using Eiromplays.IdentityServer.Application.Common.Mappings;
using Eiromplays.IdentityServer.Domain.Common;
using Eiromplays.IdentityServer.Identity.Entities;
using Eiromplays.IdentityServer.Identity.Events.Role;

namespace Eiromplays.IdentityServer.Identity.DTOs
{
    public class RoleDto : AuditableEntity, IHasDomainEvent, IMapFrom<ApplicationRole>
    {
        public string? Name { get; set; }

        private bool _done;

        public bool Done
        {
            get => _done;
            set
            {
                if (value && !_done)
                {
                    DomainEvents.Add(new RoleCompletedEvent(this));
                }

                _done = value;
            }
        }
    }
}
