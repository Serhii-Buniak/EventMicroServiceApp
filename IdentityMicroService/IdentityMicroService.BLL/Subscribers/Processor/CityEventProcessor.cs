using AutoMapper;
using IdentityMicroService.BLL.DAL.Data;
using IdentityMicroService.BLL.Subscribers.Subscribers;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace IdentityMicroService.BLL.Subscribers.Processor;

public class CityEventProcessor : ICityEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public CityEventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case CityEventType.Create:
                AddCity(message);
                break;
            case CityEventType.Update:
                UpdateCity(message);
                break;
            case CityEventType.Delete:
                DeleteCity(message);
                break;
            default:
                break;
        }

    }

    private void AddCity(string citySubscribedMessage)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var citySubscribed = JsonSerializer.Deserialize<CitySubscribed>(citySubscribedMessage);
        var city = _mapper.Map<City>(citySubscribed);

        try
        {
            if (!context.Cities.Any(c => c.Id == city.Id))
            {
                context.Cities.Add(city);
                context.SaveChanges();
                Console.WriteLine("--> City added!");
            }
            else
            {
                Console.WriteLine("--> City already exist...");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not add City to DB {ex.Message}");

        }
    }

    private void UpdateCity(string citySubscribedMessage)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var citySubscribed = JsonSerializer.Deserialize<CitySubscribed>(citySubscribedMessage);
        var city = _mapper.Map<City>(citySubscribed);

        try
        {
            if (context.Cities.Any(c => c.Id == city.Id))
            {
                context.Cities.Update(city);
                context.SaveChanges();
                Console.WriteLine("--> City updated!");
            }
            else
            {
                Console.WriteLine("--> City do not exist...");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not update City to DB {ex.Message}");
        }
    }

    private void DeleteCity(string citySubscribedMessage)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var citySubscribed = JsonSerializer.Deserialize<CitySubscribed>(citySubscribedMessage);
        var city = _mapper.Map<City>(citySubscribed);

        try
        {
            if (context.Cities.Any(c => c.Id == city.Id))
            {
                context.Cities.Remove(city);
                context.SaveChanges();
                Console.WriteLine("--> City deleted!");
            }
            else
            {
                Console.WriteLine("--> City do not exist...");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not delete City to DB {ex.Message}");

        }
    }


    private static CityEventType DetermineEvent(string notifactionMessage)
    {
        Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<ModelPublished>(notifactionMessage)!;

        switch (eventType.Event)
        {
            case "City_Create":
                Console.WriteLine("--> City Create Event Detected");
                return CityEventType.Create;
            case "City_Update":
                Console.WriteLine("--> City Update Event Detected");
                return CityEventType.Update;
            case "City_Delete":
                Console.WriteLine("--> City Delete Event Detected");
                return CityEventType.Delete;
            default:
                Console.WriteLine("--> Could not determine the event type");
                return CityEventType.Undetermined;
        }
    }

}

public enum CityEventType
{
    Create,
    Update,
    Delete,
    Undetermined
}
