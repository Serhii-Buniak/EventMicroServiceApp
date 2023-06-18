namespace CityMicroService.BLL.DTOs;

public class CityRequestDTO
{
    public string Name { get; set; } = null!;
    public long CountryId { get; set; }
}