using IdentityMicroService.BLL.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityMicroService.DAL.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.Property(prop => prop.Id)
            .ValueGeneratedNever();

        builder.HasMany(city => city.Users)
            .WithOne(user => user.City)
            .OnDelete(DeleteBehavior.SetNull);
            
    }
}
