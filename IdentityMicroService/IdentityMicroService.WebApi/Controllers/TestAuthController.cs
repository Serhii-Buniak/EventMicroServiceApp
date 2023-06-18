using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IdentityMicroService.BLL.Constants.AuthorizationConfigs;

namespace IdentityMicroService.WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TestAuthController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult Authorize()
    {
        return Ok();
    }    
    
    [HttpGet]
    [Authorize(Roles = $"{Moderator}")]
    public IActionResult AuthorizeModerator()
    {
        return Ok();
    }
}
