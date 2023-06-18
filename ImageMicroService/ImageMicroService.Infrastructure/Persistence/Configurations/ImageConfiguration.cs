using ImageMicroService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageMicroService.Infrastructure.Persistence.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(300)
            .IsRequired();

        builder.HasIndex(p => p.Name)
            .IsUnique();
    }
}