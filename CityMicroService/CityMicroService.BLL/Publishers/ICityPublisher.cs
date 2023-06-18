using CityMicroService.BLL.DTOs;

namespace CityMicroService.BLL.Publishers
{
    public interface ICityPublisher
    {
        void CreateEvent(CityDTO city);
        void DeleteEvent(CityDTO city);
        void UpdateEvent(CityDTO city);
    }
}