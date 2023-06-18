using System.Text.Json.Serialization;

namespace IdentityMicroService.BLL.Dtos;

public class AuthTokensDto
{
    public string Token { get; set; } = null!;
    public RefreshTokenDto? RefreshToken { get; set; }
}
