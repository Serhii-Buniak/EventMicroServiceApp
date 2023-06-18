namespace IdentityMicroService.BLL.Dtos;

public class CityDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long CountryId { get; set; }
}
