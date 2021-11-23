using Eiromplays.IdentityServer.Application.Common.Mappings;
using Eiromplays.IdentityServer.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Identity.DTOs
{
    public class UserLoginDto : IMapFrom<ApplicationUserLogin>
    {
        public string? UserName { get; set; }

        public string? ProviderKey { get; set; }

        public string? LoginProvider { get; set; }

        public string? ProviderDisplayName { get; set; }
    }
}