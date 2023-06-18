using EventMicroService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventMicroService.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(prop => prop.Id)
            .ValueGeneratedNever();  
        
        builder.HasMany(user => user.Events)
            .WithOne(@event => @event.User)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
