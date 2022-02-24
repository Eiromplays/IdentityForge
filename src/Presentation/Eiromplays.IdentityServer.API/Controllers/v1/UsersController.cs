using System.Security.Claims;
using System.Text.Json;
using Eiromplays.IdentityServer.Application.Common.Models;
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
    private readonly ILogger<UsersController> _logger;

    public UsersController(IIdentityService identityService, ILogger<UsersController> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<PaginatedList<UserDto>> Get(string? search, int pageIndex = 1, int pageSize = 10)
    {
        return await _identityService.GetUsersAsync(search, pageIndex, pageSize);
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] UserDto userDto)
    {
        var user = await _identityService.FindUserByIdAsync(id);

        if (user is null) return NotFound();

        await _identityService.UpdateUserAsync(userDto);

        return NoContent();
    }
}