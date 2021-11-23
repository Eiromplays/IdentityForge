using Eiromplays.IdentityServer.Application.Common.Mappings;
using Eiromplays.IdentityServer.Identity.Entities;

namespace Eiromplays.IdentityServer.Identity.DTOs
{
    public class RoleDto : IMapFrom<ApplicationRole>
    {
        public string? Name { get; set; }
    }
}
