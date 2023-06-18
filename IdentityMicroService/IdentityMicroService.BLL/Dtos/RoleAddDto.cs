using System.ComponentModel.DataAnnotations;

namespace IdentityMicroService.BLL.Dtos;

public class RoleAddDto
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    public string Role { get; set; } = null!;
}
