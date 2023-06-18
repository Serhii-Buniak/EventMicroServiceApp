using AutoMapper;
using IdentityMicroService.BLL.DAL.Data;
using IdentityMicroService.BLL.Dtos;
using IdentityMicroService.BLL.Protos;

namespace IdentityMicroService.BLL.Profiles;

public class ApplicationUserProfile : Profile
{
	public ApplicationUserProfile()
	{
        CreateMap<ApplicationUser, UserDto>();
        CreateMap<UserDto, GrpcUserModel>();
        CreateMap<RegisterDto, ApplicationUser>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Image, opt => opt.Ignore());
    }
}