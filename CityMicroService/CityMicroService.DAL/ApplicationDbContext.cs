using CityMicroService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityMicroService.DAL;

public class ApplicationDbContext : DbContext
{
    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<City> Cities { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        #region Validation
        builder.ApplyConfiguration(new CountryValidation());
        builder.ApplyConfiguration(new CityValidation());
        #endregion

        #region SeedData
        builder.ApplyConfiguration(new CountrySeeder());
        builder.ApplyConfiguration(new CitySeeder());
        #endregion

        base.OnModelCreating(builder);
    }
}