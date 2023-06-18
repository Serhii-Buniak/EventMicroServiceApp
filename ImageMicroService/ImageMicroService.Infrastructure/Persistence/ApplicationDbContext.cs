using ImageMicroService.Application.Common.Interfaces;
using ImageMicroService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ImageMicroService.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Image> Images => Set<Image>();
    public DbSet<Gallery> Galleries => Set<Gallery>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}