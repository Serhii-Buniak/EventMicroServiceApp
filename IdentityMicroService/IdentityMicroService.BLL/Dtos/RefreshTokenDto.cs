namespace IdentityMicroService.BLL.Dtos;

public class RefreshTokenDto
{
    public string Token { get; set; } = null!;
    public DateTime Expiration { get; set; }
}
