namespace IdentityMicroService.BLL.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public CityDto? City { get; set; } = null!;
    public ImageDto? Image { get; set; } = null!;
}
