namespace CityMicroService.BLL.DTOs;

public class CountryPublished : ModelPublished
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
}
