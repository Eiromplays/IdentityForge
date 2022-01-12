using Eiromplays.IdentityServer.Quickstart.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eiromplays.IdentityServer.Extensions;

public static class ControllerExtensions
{
    public static IActionResult RedirectToLocal(this Controller controller, string returnUrl)
    {
        if (controller.Url.IsLocalUrl(returnUrl))
        {
            return controller.Redirect(returnUrl);
        }

        return controller.RedirectToAction(nameof(HomeController.Index), "Home");
    }

    public static void AddErrors(this Controller controller, IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            controller.ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    public static void AddErrors(this Controller controller, IEnumerable<string> errors)
    {
        foreach (var error in errors)
        {
            controller.ModelState.AddModelError(string.Empty, error);
        }
    }
}