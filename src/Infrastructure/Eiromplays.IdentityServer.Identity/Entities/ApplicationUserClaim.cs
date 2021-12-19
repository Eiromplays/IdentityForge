using Eiromplays.IdentityServer.Application.Common.Mappings;
using Eiromplays.IdentityServer.Application.Identity.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Entities
{
    public class ApplicationUserClaim : IdentityUserClaim<string>, IMap<UserClaimDto>
    {
    }
}
