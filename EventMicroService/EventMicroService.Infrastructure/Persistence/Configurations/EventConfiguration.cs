using EventMicroService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventMicroService.Infrastructure.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.HasIndex(p => p.Name)
            .IsUnique();
    }
}
