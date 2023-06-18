using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityMicroService.DAL.Entities;

internal class CitySeeder : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasData(new List<City>()
        {
            new() {Id = 1, Name = "City 1", CountryId = 1},
            new() {Id = 2, Name = "City 2", CountryId = 2},
            new() {Id = 3, Name = "City 3", CountryId = 3},
        });
    }
}