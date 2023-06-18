namespace CityMicroService.BLL.DTOs;

public class CityPublished : ModelPublished
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long CountryId { get; set; }
}
