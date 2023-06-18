using EventMicroService.Application.Common.Interfaces;
using EventMicroService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EventMicroService.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Gallery> Galleries => Set<Gallery>();
    public DbSet<City> Cities => Set<City>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
