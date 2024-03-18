namespace IdentityForge.IdentityServer.Configurations;

public sealed class AccountConfiguration
{
    public const string SectionName = "Account";

    public required RegistrationConfiguration RegistrationConfiguration { get; set; }

    public required LoginConfiguration LoginConfiguration { get; set; }

    public required ProfilePictureConfiguration ProfilePictureConfiguration { get; set; }
}

public static class AccountConfigurationExtensions
{
    public static AccountConfiguration GetAccountConfiguration(this IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        return configuration.GetSection(AccountConfiguration.SectionName).Get<AccountConfiguration>() ??
               new AccountConfiguration
               {
                   RegistrationConfiguration = new RegistrationConfiguration(),
                   LoginConfiguration = new LoginConfiguration(),
                   ProfilePictureConfiguration = new ProfilePictureConfiguration()
               };
    }
}