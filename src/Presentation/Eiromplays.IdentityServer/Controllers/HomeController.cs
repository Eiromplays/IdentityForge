// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

using Eiromplays.IdentityServer.Application.Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eiromplays.IdentityServer.Controllers;

[SecurityHeaders]
[AllowAnonymous]
public class HomeController : Controller
{
    [Microsoft.AspNetCore.Mvc.HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}