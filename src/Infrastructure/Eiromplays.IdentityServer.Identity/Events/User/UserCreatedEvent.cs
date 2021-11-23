using Eiromplays.IdentityServer.Domain.Common;
using Eiromplays.IdentityServer.Identity.Entities;

namespace Eiromplays.IdentityServer.Identity.Events.User
{
    public class UserCreatedEvent : DomainEvent
    {
        public UserCreatedEvent(ApplicationUser user)
        {
            User = user;
        }

        public ApplicationUser User { get; }
    }
}
