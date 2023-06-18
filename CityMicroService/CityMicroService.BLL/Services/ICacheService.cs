using CityMicroService.DAL.Entities;

namespace CityMicroService.BLL.Services
{
    public interface ICacheService
    {
        IEnumerable<City>? GetCities();
        IEnumerable<Country>? GetCountries();
        IEnumerable<City> SetCities(IEnumerable<City> cities, int seconds);
        IEnumerable<Country> SetCountries(IEnumerable<Country> cities, int seconds);
    }
}