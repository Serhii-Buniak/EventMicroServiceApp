using static CityMicroService.Test.BLL.Services.CountryServiceTestHelper;

namespace CityMicroService.Test.BLL.Services;

internal static class CityServiceTestHelper
{
    static internal IEnumerable<CityDTO> GetCitiesDTOs()
    {
        return new CityDTO[]
        {
            new() { Id = 1, Name = "City1", Country = GetCountriesDTOs().First(c => c.Id == 1)},
            new() { Id = 2, Name = "City2", Country = GetCountriesDTOs().First(c => c.Id == 2)},
            new() { Id = 3, Name = "City3", Country = GetCountriesDTOs().First(c => c.Id == 3)},
        }.AsEnumerable();
    }  
    
    static internal IEnumerable<City> GetCities()
    {
        return GetCitiesDTOs().Select(c => new City { Id = c.Id, Name = c.Name });
    }
    
    static internal CityDTO? GetCityDTOById(long id) => GetCitiesDTOs().SingleOrDefault(c => c.Id ==id);
    static internal City? GetCityById(long id) => GetCities().SingleOrDefault(c => c.Id == id);

}
