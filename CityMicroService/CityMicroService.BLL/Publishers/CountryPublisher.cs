using AutoMapper;
using CityMicroService.BLL.DTOs;

namespace CityMicroService.BLL.Publishers;

public class CountryPublisher : ICountryPublisher
{
    private readonly IMessageBusClient _client;
    private readonly IMapper _mapper;

    public CountryPublisher(IMessageBusClient client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public void CreateEvent(CountryDTO country) => SendEvent(country, "Create");
    public void DeleteEvent(CountryDTO country) => SendEvent(country, "Delete");
    public void UpdateEvent(CountryDTO country) => SendEvent(country, "Update");


    private void SendEvent(CountryDTO country, string eventAction)
    {
        ModelPublished modelPublished = _mapper.Map<CountryPublished>(country);
        modelPublished.Event = $"Country_{eventAction}";

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
