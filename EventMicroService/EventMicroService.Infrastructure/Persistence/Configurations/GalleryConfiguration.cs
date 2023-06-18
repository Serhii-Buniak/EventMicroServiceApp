using EventMicroService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventMicroService.Infrastructure.Persistence.Configurations;

public class GalleryConfiguration : IEntityTypeConfiguration<Gallery>
{
    public void Configure(EntityTypeBuilder<Gallery> builder)
    {
        builder.Property(prop => prop.Id)
            .ValueGeneratedNever();  
        
    }
}