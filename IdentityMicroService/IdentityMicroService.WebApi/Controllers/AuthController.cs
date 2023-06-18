using IdentityMicroService.BLL.Dtos;
using IdentityMicroService.BLL.Exceptions;
using IdentityMicroService.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMicroService.WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _userService;

    public AuthController(IAuthService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult> Register([FromForm] RegisterDto model)
    {
        try
        {
            await _userService.RegisterAsync(model);
            return Ok();
        }
        catch (ValidationModelException ex)
        {
            return BadRequest(ex.Errors);
        }
    }

    [HttpPost]
    public async Task<IActionResult> LogIn(LogInDto model)
    {
        try
        {
            string? refreshToken = GetRefreshTokenInCookies();
            AuthTokensDto result = await _userService.LogInAsync(model, refreshToken);

            if (result.RefreshToken != null)
            {
                SetRefreshTokenInCookies(result.RefreshToken.Token, result.RefreshToken.Expiration);
            }

            return Ok(result);
        }
        catch (NotFoundException)
        {
            return BadRequest("Email or password uncorrect");
        }
        catch (ValidationModelException)
        {
            return BadRequest("Email or password uncorrect");
        }
    }

    [HttpPut]
    public async Task<IActionResult> RefreshTokens()
    {
        string? refreshToken = GetRefreshTokenInCookies();

        if (string.IsNullOrEmpty(refreshToken))
        {
            return BadRequest(new { message = "RefreshToken is required" });
        }

        try
        {
            AuthTokensDto authTokens = await _userService.RefreshTokenAsync(refreshToken);

            if (authTokens.RefreshToken != null)
            {
                SetRefreshTokenInCookies(authTokens.RefreshToken.Token, authTokens.RefreshToken.Expiration);
            }

            return Ok(authTokens);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpDelete]
    public async Task<IActionResult> LogOut()
    {
        string? refreshToken = GetRefreshTokenInCookies();

        if (string.IsNullOrEmpty(refreshToken))
        {
            return BadRequest(new { message = "RefreshToken is required" });
        }

        try
        {
            await _userService.RevokeToken(refreshToken);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    private void SetRefreshTokenInCookies(string token, DateTimeOffset expires)
    {
        CookieOptions cookieOptions = new()
        {
            HttpOnly = true,
            Expires = expires,
        };

        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }

    private string? GetRefreshTokenInCookies()
    {
        return Request.Cookies["refreshToken"];
    }
}
