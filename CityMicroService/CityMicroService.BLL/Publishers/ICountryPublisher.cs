using CityMicroService.BLL.DTOs;

namespace CityMicroService.BLL.Publishers
{
    public interface ICountryPublisher
    {
        void CreateEvent(CountryDTO country);
        void DeleteEvent(CountryDTO country);
        void UpdateEvent(CountryDTO country);
    }
}