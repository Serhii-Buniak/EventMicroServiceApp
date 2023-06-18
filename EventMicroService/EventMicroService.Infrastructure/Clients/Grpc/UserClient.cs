using AutoMapper;
using EventMicroService.Application.Common.Interfaces;
using EventMicroService.Domain.Entities;
using EventMicroService.Infrastructure.Protos;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace EventMicroService.Infrastructure.Clients.Grpc;

public class UserClient : IUserClient
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly GrpcChannel _channel;
    private readonly GrpcUser.GrpcUserClient _client;

    public UserClient(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;

        _channel = GrpcChannel.ForAddress(_configuration["GrpcUser"]);
        _client = new GrpcUser.GrpcUserClient(_channel);
    }

    public IEnumerable<User> GetAllUsers()
    {
        Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcUser"]}: GetAllUsers");

        GetAllUserRequest request = new();
        IEnumerable<GrpcUserModel> users = _client.GetAllUsers(request).Users.AsEnumerable();

        var entities = _mapper.Map<IEnumerable<User>>(users).ToList();
        entities.ForEach(u =>
        {
            u.CityId = u.CityId == 0 ? null : u.CityId;
        });

        return entities;
    }
}
