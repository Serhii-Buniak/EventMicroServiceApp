using AutoMapper;
using CityMicroService.BLL.Protos;
using CityMicroService.DAL.Entities;

namespace CityMicroService.BLL.DTOs;

internal class CountryMapping : Profile
{
    public CountryMapping()
    {
        CreateMap<Country, CountryDTO>().ReverseMap();
        CreateMap<Country, CountryRequestDTO>().ReverseMap();
        CreateMap<GrpcCountryModel, CountryDTO>().ReverseMap();
        CreateMap<CountryPublished, CountryDTO>().ReverseMap();
    }
}