namespace CityMicroService.BLL.DTOs;

public class CityDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public CountryDTO Country { get; set; } = null!;
}
