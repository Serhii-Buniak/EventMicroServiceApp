using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CityMicroService.WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TestAuth2Controller : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult Authorize()
    {
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = "Moderator")]
    public IActionResult AuthorizeModerator()
    {
        return Ok();
    }
}
