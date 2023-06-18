using EventMicroService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventMicroService.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; }
    DbSet<Event> Events { get; }
    DbSet<City> Cities { get; }
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
