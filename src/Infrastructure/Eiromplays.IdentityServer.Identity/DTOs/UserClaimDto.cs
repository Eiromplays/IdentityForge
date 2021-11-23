using Eiromplays.IdentityServer.Application.Common.Mappings;
using Eiromplays.IdentityServer.Identity.Entities;

namespace Eiromplays.IdentityServer.Identity.DTOs
{
    public class UserClaimDto : IMapFrom<ApplicationUserClaim>
    {
        public int Id { get; set; }

        public int ClaimId { get; set; }

        public string? UserId { get; set; }

        public string? ClaimType { get; set; }

        public string? ClaimValue { get; set; }
    }
}
