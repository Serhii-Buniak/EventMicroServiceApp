using AutoMapper;
using IdentityMicroService.BLL.DAL.Data;
using IdentityMicroService.BLL.Dtos;
using Microsoft.AspNetCore.Identity;

namespace IdentityMicroService.BLL.Profiles;

public class ApplicationRoleProfile : Profile
{
	public ApplicationRoleProfile()
	{
        CreateMap<ApplicationRole, RoleDto>();
    }
}
