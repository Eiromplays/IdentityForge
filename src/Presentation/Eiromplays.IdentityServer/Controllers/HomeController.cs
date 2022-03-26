// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Application.Common.Security;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eiromplays.IdentityServer.Controllers;

[SecurityHeaders]
[AllowAnonymous]
public class HomeController : Controller
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(IIdentityServerInteractionService interaction, IWebHostEnvironment environment,
        ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, IServiceProvider serviceProvider)
    {
        _interaction = interaction;
        _environment = environment;
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
        Console.WriteLine($"Users: {_userManager.Users.Count()}");
        return View();
    }

    /// <summary>
    /// Shows the error page
    /// </summary>
    public async Task<IActionResult> Error(string errorId)
    {
        var vm = new ErrorViewModel();

        // retrieve error details from identityserver
        var message = await _interaction.GetErrorContextAsync(errorId);
        if (message is null) return View("Error", vm);
        vm.Error = message;

        if (!_environment.IsDevelopment())
        {
            // only show in development
            message.ErrorDescription = null;
        }

        return View("Error", vm);
    }
}