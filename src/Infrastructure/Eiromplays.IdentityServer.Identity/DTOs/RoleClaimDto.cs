using Eiromplays.IdentityServer.Application.Common.Mappings;
using Eiromplays.IdentityServer.Identity.Entities;

namespace Eiromplays.IdentityServer.Identity.DTOs
{
    public class RoleClaimDto : IMapFrom<ApplicationRoleClaim>
    {
        public int Id { get; set; }

        public string? RoleId { get; set; }

        public string? ClaimType { get; set; }

        public string? ClaimValue { get; set; }
    }
}