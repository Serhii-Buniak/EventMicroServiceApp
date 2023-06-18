using ImageMicroService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageMicroService.Infrastructure.Persistence.Configurations;

public class GalleryConfiguration : IEntityTypeConfiguration<Gallery>
{
    public void Configure(EntityTypeBuilder<Gallery> builder)
    {
        builder
            .HasMany(g => g.Images)
            .WithOne(i => i.Gallery)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(p => p.Name)
            .IsUnique();
    }
}
