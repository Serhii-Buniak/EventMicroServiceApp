using IdentityMicroService.BLL.Dtos;
using System.Net;

namespace IdentityMicroService.BLL.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto model);
    Task<AuthTokensDto> LogInAsync(LogInDto model, string? currRefreshToken);
    Task<AuthTokensDto> RefreshTokenAsync(string jwtToken);
    Task RevokeToken(string token);
}
