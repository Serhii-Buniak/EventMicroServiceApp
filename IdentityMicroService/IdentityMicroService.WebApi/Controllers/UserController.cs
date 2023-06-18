using IdentityMicroService.BLL.Exceptions;
using IdentityMicroService.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IdentityMicroService.BLL.Constants.AuthorizationConfigs;

namespace IdentityMicroService.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public partial class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;

    public UserController(IUserService userService, IRoleService roleService)
    {
        _userService = userService;
        _roleService = roleService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{Moderator}, {Administrator}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        return Ok(await _userService.GetByIdAsync(id));
    }

    [HttpGet]
    [Authorize(Roles = $"{Moderator}, {Administrator}")]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _userService.GetAllAsync());
    }

    [HttpGet("{userId}/Role")]
    [Authorize(Roles = $"{Moderator}, {Administrator}")]
    public async Task<IActionResult> GetRolesForUser(Guid userId)
    {
        try
        {
            return Ok(await _roleService.GetRolesForUserAsync(userId));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{userId}/Role/{roleId}")]
    [Authorize(Roles = Administrator)]
    public async Task<IActionResult> AddRoleForUser(Guid userId, Guid roleId)
    {
        try
        {
            await _roleService.AddRoleForUserAsync(userId, roleId);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ValidationModelException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{userId}/Role/{roleId}")]
    [Authorize(Roles = Administrator)]
    public async Task<IActionResult> RemoveRoleForUser(Guid userId, Guid roleId)
    {
        try
        {
            await _roleService.RemoveRoleForUserAsync(userId, roleId);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ValidationModelException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
