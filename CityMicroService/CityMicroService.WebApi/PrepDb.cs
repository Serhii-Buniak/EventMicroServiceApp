using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CityMicroService.DAL;

public static class PrepDb
{
    public static void PrepPopulation(WebApplicationBuilder builder)
    {
        using ServiceProvider provider = builder.Services.BuildServiceProvider();
        SeedData(provider.GetRequiredService<ApplicationDbContext>(), builder.Environment);
    }

    private static void SeedData(ApplicationDbContext context, IWebHostEnvironment env)
    {
        context.Database.Migrate();
    }
}
