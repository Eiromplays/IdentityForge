using IdentityForge.IdentityServer.Domain.Users;

namespace IdentityForge.IdentityServer.Configurations;

public sealed class LoginConfiguration
{
    public const string SectionName = "Login";

    public LoginMethodEnum LoginPolicy { get; set; } = LoginMethodEnum.All;
}