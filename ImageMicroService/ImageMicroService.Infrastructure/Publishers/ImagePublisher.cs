using AutoMapper;
using ImageMicroService.Application.Common.Interfaces;
using ImageMicroService.Application.Common.Models;

namespace ImageMicroService.Infrastructure.Publishers;

public class ImagePublisher : IImagePublisher
{
    private readonly MessageBusClient _client;
    private readonly IMapper _mapper;

    public ImagePublisher(MessageBusClient client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public void DeleteEvent(ImageDto image) => SendEvent(image, "Delete");


    private void SendEvent(ImageDto image, string eventAction)
    {
        ModelPublished modelPublished = _mapper.Map<ImagePublished>(image);
        modelPublished.Event = $"Image_{eventAction}";

        if (_client.ConnectionIsOpen)
        {
            Console.WriteLine("--> RabbitMQ Connection Open, sending message");
            _client.SendMassage(modelPublished);
        }
        else
        {
            Console.WriteLine("--> RabbinMq connection is closed, not sending");
        }
    }
}
