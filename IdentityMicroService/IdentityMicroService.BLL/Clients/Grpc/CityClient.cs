using AutoMapper;
using Grpc.Net.Client;
using IdentityMicroService.BLL.DAL.Data;
using IdentityMicroService.BLL.Protos;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Channels;

namespace IdentityMicroService.BLL.Clients.Grpc;

public class CityClient : ICityClient
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    private readonly GrpcChannel _channel;
    private readonly GrpcCity.GrpcCityClient _client;

    public CityClient(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;

        _channel = GrpcChannel.ForAddress(_configuration["GrpcCity"]);
        _client = new GrpcCity.GrpcCityClient(_channel);
    }

    public IEnumerable<GrpcCityModel> GetAllCities()
    {
        Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcCity"]}: GetAllCities");

        GetAllRequest request = new();

        return _client.GetAllCities(request).City.AsEnumerable();
    }   
    
    public IEnumerable<GrpcCountryModel> GetAllCountries()
    {
        Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcCity"]}: GetAllCountries");

        GetAllRequest request = new();

        return _client.GetAllCountries(request).Country.AsEnumerable();
    }
}