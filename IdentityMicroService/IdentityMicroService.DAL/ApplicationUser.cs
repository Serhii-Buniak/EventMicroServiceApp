using Microsoft.AspNetCore.Identity;

namespace IdentityMicroService.BLL.DAL.Data;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public long? CityId { get; set; }
    public City? City { get; set; }
    public long? ImageId { get; set; }
    public Image? Image { get; set; } = null!;
    public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
