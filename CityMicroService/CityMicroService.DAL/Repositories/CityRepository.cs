using CityMicroService.DAL.Entities;
using CityMicroService.DAL.RepositoryBase;

namespace CityMicroService.DAL.Repositories;

public class CityRepository : RepositoryBase<City>, ICityRepository
{
    public CityRepository(ApplicationDbContext appDbContext) : base(appDbContext)
    {

    }
}