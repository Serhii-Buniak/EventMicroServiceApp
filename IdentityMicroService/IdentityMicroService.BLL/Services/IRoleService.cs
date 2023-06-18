using IdentityMicroService.BLL.Dtos;

namespace IdentityMicroService.BLL.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAllAsync();
    Task<RoleDto> GetByIdAsync(Guid id);
    Task<IEnumerable<RoleDto>> GetRolesForUserAsync(Guid userId);
    Task AddRoleForUserAsync(Guid userId, Guid roleId);
    Task RemoveRoleForUserAsync(Guid userId, Guid roleId);
}