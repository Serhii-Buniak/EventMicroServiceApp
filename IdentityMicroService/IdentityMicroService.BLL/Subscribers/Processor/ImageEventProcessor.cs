using AutoMapper;
using IdentityMicroService.BLL.DAL.Data;
using IdentityMicroService.BLL.Subscribers.Subscribers;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace IdentityMicroService.BLL.Subscribers.Processor;

public class ImageEventProcessor : IImageEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public ImageEventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case ImageEventType.Delete:
                DeleteImage(message);
                break;
            default:
                break;
        }
    }

    private void DeleteImage(string imageSubscribedMessage)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        Console.WriteLine("adsadasdsa");
        var imageSubscribed = JsonSerializer.Deserialize<ImageSubscribed>(imageSubscribedMessage);
        Console.WriteLine("adsadasdsa 2");
        var image = _mapper.Map<Image>(imageSubscribed);

        try
        {
            if (context.Images.Any(c => c.Id == image.Id))
            {
                context.Images.Remove(image);
                context.SaveChanges();
                Console.WriteLine("--> Image deleted!");
            }
            else
            {
                Console.WriteLine("--> Image do not exist...");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not delete Image to DB {ex.Message}");

        }
    }


    private static ImageEventType DetermineEvent(string notifactionMessage)
    {
        Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<ModelPublished>(notifactionMessage)!;

        switch (eventType.Event)
        {
            case "Image_Delete":
                Console.WriteLine("--> Image Delete Event Detected");
                return ImageEventType.Delete;
            default:
                Console.WriteLine("--> Could not determine the event type");
                return ImageEventType.Undetermined;
        }
    }


}

enum ImageEventType
{
    Delete,
    Undetermined
}