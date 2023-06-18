using AutoMapper;
using CityMicroService.BLL.DTOs;
using CityMicroService.BLL.Protos;
using CityMicroService.BLL.Services;
using Grpc.Core;

namespace CityMicroService.WebApi.GrpcControllers;

public class CityGrpcController : GrpcCity.GrpcCityBase
{
    private readonly ICityService _cityService;
    private readonly ICountryService _countryService;
    private readonly IMapper _mapper;

    public CityGrpcController(ICityService cityService, ICountryService countryService, IMapper mapper)
    {
        _cityService = cityService;
        _countryService = countryService;
        _mapper = mapper;
    }

    public override async Task<CitiesResponse> GetAllCities(GetAllRequest request, ServerCallContext context)
    {
        CitiesResponse response = new();
       
        IEnumerable<CityDTO> cities = await _cityService.GetAllAsync();
        var models = _mapper.Map<IEnumerable<GrpcCityModel>>(cities);

        response.City.AddRange(models);
        return response;
    }

    public override async Task<CountriesResponse> GetAllCountries(GetAllRequest request, ServerCallContext context)
    {
        CountriesResponse response = new();

        IEnumerable<CountryDTO> countries = await _countryService.GetAllAsync();
        var models = _mapper.Map<IEnumerable<GrpcCountryModel>>(countries);

        response.Country.AddRange(models);
        return response;
    }
}