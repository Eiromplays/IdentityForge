using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Common.Security;
using Eiromplays.IdentityServer.Application.Identity.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Identity.DTOs.Role;
using Microsoft.AspNetCore.Mvc;

namespace Eiromplays.IdentityServer.API.Controllers.v1;

[Authorize(Policy = "RequireInteractiveUser")]
[ApiController]
[ApiVersion("1.0")]
[Route("v1/[controller]")]
[Route("[controller]")]
public class RolesController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public RolesController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpGet]
    public async Task<PaginatedList<RoleDto>> Get(string? search, int pageIndex = 1, int pageSize = 10)
    {
        return await _identityService.GetRolesAsync(search, pageIndex, pageSize);
    }


    [HttpGet("{id}")]
    public async Task<RoleDto?> Get(string id)
    {
        var role = await _identityService.FindRoleByIdAsync(id);

        return role;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RoleDto roleDto)
    {
        var (_, roleId) = await _identityService.CreateRoleAsync(roleDto);

        return Created(Url.Action(nameof(Get), new { id = roleId })!, roleDto);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] RoleDto roleDto)
    {
        var role = await _identityService.FindRoleByIdAsync(id);

        if (role is null) return NotFound();

        await _identityService.UpdateRoleAsync(roleDto);

        return NoContent();
    }
}