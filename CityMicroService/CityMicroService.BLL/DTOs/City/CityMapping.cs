using AutoMapper;
using CityMicroService.BLL.Protos;
using CityMicroService.DAL.Entities;

namespace CityMicroService.BLL.DTOs;

internal class CityMapping : Profile
{
    public CityMapping()
    {
        CreateMap<City, CityDTO>().ReverseMap();
        CreateMap<City, CityRequestDTO>().ReverseMap();
        CreateMap<GrpcCityModel, CityDTO>().ReverseMap();
        CreateMap<CityPublished, CityDTO>().ReverseMap();
    }
}