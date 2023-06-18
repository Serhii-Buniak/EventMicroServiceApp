using ImageMicroService.Application.Common.Interfaces;
using Azure.Storage.Blobs;
using ImageMicroService.Infrastructure.Persistence;
using ImageMicroService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ImageMicroService.Infrastructure.Publishers;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("ImageMicroService"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Database"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        services.AddSingleton<MessageBusClient>();
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddSingleton(x => new BlobServiceClient(configuration.GetConnectionString("Storage")));
        services.AddScoped<IImageBlobService, ImageBlobService>();
        services.AddScoped<IImagePublisher, ImagePublisher>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "ImageMicroService_";
        });

        return services;
    }
}