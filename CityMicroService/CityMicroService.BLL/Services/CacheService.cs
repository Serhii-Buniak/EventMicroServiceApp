using CityMicroService.DAL.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace CityMicroService.BLL.Services;

public class CacheService : ICacheService
{
    private const string CitiesKey = "cities";
    private const string CountriesKey = "countries";

    private readonly IMemoryCache _memoryCache;

    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public IEnumerable<City> SetCities(IEnumerable<City> cities, int seconds)
    {
        return _memoryCache.Set(CitiesKey, cities, TimeSpan.FromSeconds(seconds));
    }

    public IEnumerable<City>? GetCities()
    {
        return _memoryCache.Get<IEnumerable<City>>(CitiesKey);
    }

    public IEnumerable<Country> SetCountries(IEnumerable<Country> cities, int seconds)
    {
        return _memoryCache.Set(CountriesKey, cities, TimeSpan.FromSeconds(seconds));
    }

    public IEnumerable<Country>? GetCountries()
    {
        return _memoryCache.Get<IEnumerable<Country>>(CountriesKey);
    }
}
