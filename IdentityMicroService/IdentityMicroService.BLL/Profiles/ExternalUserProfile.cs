using AutoMapper;
using IdentityMicroService.BLL.DAL.Data;
using IdentityMicroService.BLL.Dtos;
using IdentityMicroService.BLL.Protos;
using IdentityMicroService.BLL.Subscribers.Subscribers;

namespace IdentityMicroService.BLL.Profiles;

public class ExternalUserProfile : Profile
{
    public ExternalUserProfile()
    {
        CreateMap<GrpcCityModel, City>();
        CreateMap<CitySubscribed, City>();
        CreateMap<City, CityDto>();
        CreateMap<ImageDto, Image>().ReverseMap();
        CreateMap<ImageSubscribed, Image>().ReverseMap();
    }
}
