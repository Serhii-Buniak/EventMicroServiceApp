using EventMicroService.Domain.Entities;

namespace EventMicroService.Application.Common.Interfaces;

public interface ICityClient
{
    IEnumerable<City> GetAllCities();
}