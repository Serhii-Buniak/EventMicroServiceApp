using IdentityMicroService.BLL.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityMicroService.DAL.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.Property(prop => prop.Id)
            .ValueGeneratedNever();

        builder.HasMany(image => image.Users)
           .WithOne(user => user.Image)
           .OnDelete(DeleteBehavior.SetNull);
    }
}