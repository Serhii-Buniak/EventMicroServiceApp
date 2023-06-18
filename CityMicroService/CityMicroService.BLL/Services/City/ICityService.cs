using CityMicroService.BLL.DTOs;

namespace CityMicroService.BLL.Services;

public interface ICityService
{
    Task<CityDTO> CreateAsync(CityRequestDTO cityRequest);
    Task<CityDTO> DeleteAsync(long id);
    Task<IEnumerable<CityDTO>> GetAllAsync();
    Task<CityDTO> GetByIdAsync(long id);
    Task<CityDTO> UpdateAsync(long id, CityRequestDTO cityRequest);
}