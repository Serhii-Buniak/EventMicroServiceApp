using CityMicroService.BLL.Publishers;
using CityMicroService.BLL.Services;
using CityMicroService.DAL.RepositoryWrapper;

namespace CityMicroService.WebApi.StartupExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServicesList(this IServiceCollection services)
    {

        services.AddSingleton<IMessageBusClient, MessageBusClient>();

        services.AddSingleton<ICacheService, CacheService>();

        services.AddTransient<ICountryPublisher, CountryPublisher>();
        services.AddTransient<ICityPublisher, CityPublisher>();

        services.AddTransient<IRepositoryWrapper, RepositoryWrapper>();
        services.AddTransient<ICountryService, CountryService>();
        services.AddTransient<ICityService, CityService>();

        return services;
    }
}
