using Eiromplays.IdentityServer.Domain.Constants;

namespace Eiromplays.IdentityServer.Application.Common.Configurations.Account;

public class LoginConfiguration
{
    public LoginPolicy LoginPolicy { get; set; } = LoginPolicy.All;
}