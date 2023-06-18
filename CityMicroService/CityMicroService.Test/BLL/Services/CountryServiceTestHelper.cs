using CityMicroService.DAL.Entities;

namespace CityMicroService.Test.BLL.Services;

internal static class CountryServiceTestHelper
{
    static internal IEnumerable<CountryDTO> GetCountriesDTOs()
    {
        return new CountryDTO[]
        {
            new() { Id = 1, Name = "Country1" },
            new() { Id = 2, Name = "Country2" },
            new() { Id = 3, Name = "Country3" },
        }.AsEnumerable();
    }   
    
    static internal IEnumerable<Country> GetCountries()
    {
        return GetCountriesDTOs().Select(c => new Country { Id = c.Id, Name = c.Name });
    }

    static internal CountryDTO? GetCountryDTOById(long id) => GetCountriesDTOs().SingleOrDefault(c => c.Id == id);
    static internal Country? GetCountryById(long id) => GetCountries().SingleOrDefault(c => c.Id == id);
}
