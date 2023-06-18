using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityMicroService.DAL.Entities;

internal class CityValidation : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}