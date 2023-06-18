using IdentityMicroService.BLL.Dtos;

namespace IdentityMicroService.BLL.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(Guid id);
}