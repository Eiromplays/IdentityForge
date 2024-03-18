namespace IdentityForge.IdentityServer.Configurations;

public sealed class RegistrationConfiguration
{
    public const string SectionName = "Registration";

    public bool Enabled { get; set; } = true;
}