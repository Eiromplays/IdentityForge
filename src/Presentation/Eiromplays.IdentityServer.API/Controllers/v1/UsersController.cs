using Eiromplays.IdentityServer.Application.Common.Security;
using Eiromplays.IdentityServer.Application.Identity.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Identity.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace Eiromplays.IdentityServer.API.Controllers.v1;

[Authorize(Policy = "RequireInteractiveUser")]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/users")]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public UsersController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpGet]
    public async Task<UserDto?> Get()
    {
        var user = await _identityService.FindUserByIdAsync(User.FindFirst("sub")?.Value);

        return user;
    }
}