using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityMicroService.DAL.Entities;

internal class CountrySeeder : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasData(new List<Country>()
        {
            new() {Id = 1, Name = "Country 1"},
            new() {Id = 2, Name = "Country 2"},
            new() {Id = 3, Name = "Country 3"},
        });
    }
}