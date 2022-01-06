using Eiromplays.IdentityServer.Application.Common.Mappings;
using Eiromplays.IdentityServer.Application.Identity.DTOs.Role;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

public class ApplicationRoleClaim : IdentityRoleClaim<string>, IMap<RoleClaimDto>
{

}