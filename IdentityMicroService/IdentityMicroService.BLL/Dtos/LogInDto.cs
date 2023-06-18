using System.ComponentModel.DataAnnotations;

namespace IdentityMicroService.BLL.Dtos;

public class LogInDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool RemeberMe { get; set; }
}