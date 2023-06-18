using EventMicroService.Application.Common.Interfaces;
using EventMicroService.Infrastructure.Clients.Grpc;
using EventMicroService.Infrastructure.Clients.Http;
using EventMicroService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EventMicroService.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Database"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddHttpClient<GalleryClient>();

        services.AddScoped<IGalleryClient, GalleryClient>();
        services.AddScoped<ICityClient, CityClient>();
        services.AddScoped<IUserClient, UserClient>();

        return services;
    }
}
