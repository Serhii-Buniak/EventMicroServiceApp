using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityMicroService.DAL.Entities;

internal class CountryValidation : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.Property(p => p.Name)
               .HasMaxLength(50)
               .IsRequired();

        builder.HasIndex(p => p.Name)
            .IsUnique();
    }
}