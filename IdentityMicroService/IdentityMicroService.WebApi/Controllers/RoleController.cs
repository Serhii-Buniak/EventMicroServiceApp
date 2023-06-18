using IdentityMicroService.BLL.Exceptions;
using IdentityMicroService.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMicroService.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRole(Guid id)
    {
        try
        {
            return Ok(await _roleService.GetByIdAsync(id));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        return Ok(await _roleService.GetAllAsync());
    }
}