namespace Eiromplays.IdentityServer.ViewModels.Account;

public class LogoutResponse
{
    public LogoutViewModel? LogoutViewModel { get; set; }
    
    public LoggedOutViewModel? LoggedOutViewModel { get; set; }

    public LogoutResponse(LogoutViewModel? logoutViewModel = null, LoggedOutViewModel? loggedOutViewModel = null)
    {
        LogoutViewModel = logoutViewModel;
        LoggedOutViewModel = loggedOutViewModel;
    }
}