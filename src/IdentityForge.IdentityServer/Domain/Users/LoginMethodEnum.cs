using Ardalis.SmartEnum;

namespace IdentityForge.IdentityServer.Domain.Users;

public abstract class LoginMethodEnum : SmartEnum<LoginMethodEnum>
{
    public static readonly LoginMethodEnum Username = new UsernameType();
    public static readonly LoginMethodEnum Email = new EmailType();
    public static readonly LoginMethodEnum PhoneNumber = new PhoneNumberType();
    public static readonly LoginMethodEnum All = new AllType();

    protected LoginMethodEnum(string name, int value) : base(name, value)
    {
    }

    private class UsernameType : LoginMethodEnum
    {
        public UsernameType() : base("Username", 0) { }
    }

    private class EmailType : LoginMethodEnum
    {
        public EmailType() : base("Email", 1) { }
    }

    private class PhoneNumberType : LoginMethodEnum
    {
        public PhoneNumberType() : base("PhoneNumber", 2) { }
    }

    private class AllType : LoginMethodEnum
    {
        public AllType() : base("All", 3) { }
    }
}