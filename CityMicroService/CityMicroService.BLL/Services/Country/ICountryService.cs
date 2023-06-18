using CityMicroService.BLL.DTOs;

namespace CityMicroService.BLL.Services;

public interface ICountryService
{
    Task<CountryDTO> CreateAsync(CountryRequestDTO cityRequest);
    Task<CountryDTO> DeleteAsync(long id);
    Task<IEnumerable<CountryDTO>> GetAllAsync();
    Task<CountryDTO> GetByIdAsync(long id);
    Task<CountryDTO> UpdateAsync(long id, CountryRequestDTO cityRequest);
}