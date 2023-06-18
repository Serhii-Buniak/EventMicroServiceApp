using AutoMapper;
using EventMicroService.Domain.Entities;
using EventMicroService.Infrastructure.Protos;

namespace EventMicroService.Infrastructure.Profiles;

public class UserProfile : Profile
{
	public UserProfile()
	{
        CreateMap<GrpcUserModel, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => new Guid(src.Id)));
    }
}
