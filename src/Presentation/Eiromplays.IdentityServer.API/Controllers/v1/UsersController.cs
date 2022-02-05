using Eiromplays.IdentityServer.Application.Common.Security;
using Eiromplays.IdentityServer.Application.Identity.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Identity.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace Eiromplays.IdentityServer.API.Controllers.v1;

[Authorize(Policy = "RequireInteractiveUser")]
[ApiController]
[ApiVersion("1.0")]
[Route("v1/[controller]")]
[Route("[controller]")]
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

    [HttpGet("{id}")]
    public async Task<UserDto?> Get(string id)
    {
        var user = await _identityService.FindUserByIdAsync(id);

        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserDto userDto)
    {
        var (_, userId) = await _identityService.CreateUserAsync(userDto);

        return Created(Url.Action(nameof(Get), new { id = userId})!, userDto);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UserDto userDto)
    {
        var user = await _identityService.FindUserByIdAsync(User.FindFirst("sub")?.Value);

        if (user is null) return NotFound();

        await _identityService.UpdateUserAsync(userDto);

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] UserDto userDto)
    {
        var user = await _identityService.FindUserByIdAsync(id);

        if (user is null) return NotFound();

        await _identityService.UpdateUserAsync(userDto);

        return NoContent();
    }
}