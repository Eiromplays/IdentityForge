using Eiromplays.IdentityServer.Domain.Common;
using Eiromplays.IdentityServer.Identity.DTOs;

namespace Eiromplays.IdentityServer.Identity.Events.User
{
    public class UserCompletedEvent : DomainEvent
    {
        public UserCompletedEvent(UserDto userDto)
        {
            UserDto = userDto;
        }

        public UserDto UserDto { get; }
    }
}
