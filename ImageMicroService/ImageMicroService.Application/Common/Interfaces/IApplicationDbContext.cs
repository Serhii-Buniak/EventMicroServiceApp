using ImageMicroService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageMicroService.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Image> Images { get; }
    DbSet<Gallery> Galleries { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}