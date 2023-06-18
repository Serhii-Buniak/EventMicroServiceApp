using EventMicroService.Application.Common.Interfaces;
using EventMicroService.Domain.Entities;
using EventMicroService.Infrastructure.Protos;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace EventMicroService.Infrastructure.Clients.Grpc;

public class CityClient : ICityClient
{
    private readonly IConfiguration _configuration;

    private readonly GrpcChannel _channel;
    private readonly GrpcCity.GrpcCityClient _client;

    public CityClient(IConfiguration configuration)
    {
        _configuration = configuration;

        _channel = GrpcChannel.ForAddress(_configuration["GrpcCity"]);
        _client = new GrpcCity.GrpcCityClient(_channel);
    }

    public IEnumerable<City> GetAllCities()
    {
        Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcCity"]}: GetAllCities");

        GetAllRequest request = new();

        return _client.GetAllCities(request).City.Select(c => new City { Id = c.Id, Name = c.Name, CountryId = c.CountryId });
    }
}
