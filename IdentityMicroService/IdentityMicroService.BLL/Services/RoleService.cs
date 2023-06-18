using AutoMapper;
using IdentityMicroService.BLL.DAL.Data;
using IdentityMicroService.BLL.Dtos;
using IdentityMicroService.BLL.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityMicroService.BLL.Services;

public class RoleService : IRoleService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RoleService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext context, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoleDto>> GetAllAsync()
    {
        IEnumerable<ApplicationRole> roles = await _roleManager.Roles.ToListAsync();
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }

    public async Task<RoleDto> GetByIdAsync(Guid id)
    {
        ApplicationRole? role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);

        if (role is null)
        {
            throw new NotFoundException(nameof(ApplicationRole), id);
        }

        return _mapper.Map<RoleDto>(role);
    }

    public async Task<IEnumerable<RoleDto>> GetRolesForUserAsync(Guid userId)
    {
        ApplicationUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            throw new NotFoundException(nameof(ApplicationUser), userId);
        }

        IQueryable<Guid> roleIds = _context.UserRoles.Where(ur => ur.UserId == userId).Select(ur => ur.RoleId);
        IQueryable<ApplicationRole> roles = _context.Roles.Where(r => roleIds.Contains(r.Id));

        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }   
    
    public async Task AddRoleForUserAsync(Guid userId, Guid roleId)
    {
        ApplicationUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
        {
            throw new NotFoundException(nameof(ApplicationUser), userId);
        }

        ApplicationRole? role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        if (role is null)
        {
            throw new NotFoundException(nameof(ApplicationRole), roleId);
        }

        IdentityResult addRoleResult = await _userManager.AddToRoleAsync(user, role.Name);
        if (!addRoleResult.Succeeded)
        {
            throw new ValidationModelException(addRoleResult.Errors);
        }
    }    
    
    public async Task RemoveRoleForUserAsync(Guid userId, Guid roleId)
    {
        ApplicationUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
        {
            throw new NotFoundException(nameof(ApplicationUser), userId);
        }

        ApplicationRole? role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        if (role is null)
        {
            throw new NotFoundException(nameof(ApplicationRole), roleId);
        }

        IdentityResult addRoleResult = await _userManager.RemoveFromRoleAsync(user, role.Name);
        if (!addRoleResult.Succeeded)
        {
            throw new ValidationModelException(addRoleResult.Errors);
        }
    }
}