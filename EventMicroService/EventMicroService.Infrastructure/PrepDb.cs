using AutoMapper;
using EventMicroService.Application.Common.Interfaces;
using EventMicroService.Domain.Entities;
using EventMicroService.Infrastructure.Clients.Grpc;
using EventMicroService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventMicroService.Infrastructure;

public static class PrepDb
{
    public static void PrepPopulation(WebApplicationBuilder builder)
    {
        using ServiceProvider provider = builder.Services.BuildServiceProvider();
        var context = provider.GetRequiredService<ApplicationDbContext>();
        var cityClient = provider.GetRequiredService<ICityClient>();
        var userClient = provider.GetRequiredService<IUserClient>();
        SeedData(context, cityClient, userClient);
    }

    private static void SeedData(ApplicationDbContext context, ICityClient cityClient, IUserClient userClient)
    {
        context.Database.Migrate();

        var externalCities = cityClient.GetAllCities();
        var existCities = context.Cities.AsNoTracking().AsEnumerable();

        IEnumerable<City> diffCities = existCities.Except(externalCities);

        context.Cities.RemoveRange(diffCities);

        foreach (City city in externalCities)
        {
            if (!context.Cities.Any(c => c.Id == city.Id))
            {

                context.Cities.Add(city);
            }
            else
            {
                context.Cities.Update(city);
            }
        }
        context.SaveChanges();

        var externalUsers = userClient.GetAllUsers();
        var existUsers = context.Users.AsNoTracking().AsEnumerable();

        IEnumerable<User> diffUsers = existUsers.Except(externalUsers);

        context.Users.RemoveRange(diffUsers);

        foreach (User user in externalUsers)
        {
            if (!context.Users.Any(c => c.Id == user.Id))
            {

                context.Users.Add(user);
            }
            else
            {
                context.Users.Update(user);
            }
        }

        context.SaveChanges();
    }
}