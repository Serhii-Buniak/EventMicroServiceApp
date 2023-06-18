using AutoMapper;
using CityMicroService.BLL.DTOs;

namespace CityMicroService.BLL.Publishers;

public class CityPublisher : ICityPublisher
{
    private readonly IMessageBusClient _client;
    private readonly IMapper _mapper;

    public CityPublisher(IMessageBusClient client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public void CreateEvent(CityDTO city) => SendEvent(city, "Create");
    public void DeleteEvent(CityDTO city) => SendEvent(city, "Delete");
    public void UpdateEvent(CityDTO city) => SendEvent(city, "Update");


    private void SendEvent(CityDTO city, string eventAction)
    {
        ModelPublished modelPublished = _mapper.Map<CityPublished>(city);
        modelPublished.Event = $"City_{eventAction}";

        if (_client.ConnectionIsOpen)
        {
            Console.WriteLine("--> RabbitMQ Connection Open, sending message @CreateEvent...");
            _client.SendMassage(modelPublished);
        }
        else
        {
            Console.WriteLine("--> RabbinMq connection is closed, not sending @CreateEvent");
        }
    }
}