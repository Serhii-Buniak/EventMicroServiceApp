using AutoMapper;
using IdentityMicroService.BLL.Clients.Grpc;
using IdentityMicroService.BLL.DAL.Data;
using IdentityMicroService.BLL.Protos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using static IdentityMicroService.BLL.Constants.AuthorizationConfigs;

namespace IdentityMicroService.WebApi;

public static class PrepDb
{
    public static void PrepPopulation(WebApplicationBuilder builder)
    {
        using ServiceProvider provider = builder.Services.BuildServiceProvider();
        var context = provider.GetRequiredService<ApplicationDbContext>();
        var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = provider.GetRequiredService<RoleManager<ApplicationRole>>();
        var cityClient = provider.GetRequiredService<ICityClient>();
        var mapper = provider.GetRequiredService<IMapper>();

        SeedData(context, roleManager, userManager, cityClient, mapper);
    }

    private static void SeedData(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ICityClient cityClient, IMapper mapper)
    {
        context.Database.Migrate();

        var externalCities = mapper.Map<IEnumerable<City>>(cityClient.GetAllCities());
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


        foreach (Roles key in roleDict.Keys)
        {
            string role = roleDict[key];
            if (!roleManager.Roles.Any(r => r.NormalizedName == role.ToUpper()))
            {
                roleManager.CreateAsync(new ApplicationRole(role)).Wait();
            }
        }

        if (!context.Users.Any(u => u.UserName == "Admin"))
        {
            ApplicationUser user = new()
            {
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "Admin",
                Email = "admin@example.com",
            };

            userManager.CreateAsync(user, "admin123").Wait();

            userManager.AddToRoleAsync(user, roleDict[Roles.Administrator]).Wait();
        }
    }
}
