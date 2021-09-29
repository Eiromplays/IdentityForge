using EntityFrameworkCore.EncryptColumn.Attribute;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string? DisplayName { get; set; }

        [ProtectedPersonalData]
        [EncryptColumn]
        public override string? Email { get; set; }

        [PersonalData]
        public string? ProfilePicture { get; set; }

        [PersonalData]
        [EncryptColumn]
        public string? GravatarEmail { get; set; }

        [PersonalData]
        [EncryptColumn]
        public decimal Credits { get; set; }

        [PersonalData]
        [EncryptColumn]
        public string? DiscordId { get; set; }
    }
}
