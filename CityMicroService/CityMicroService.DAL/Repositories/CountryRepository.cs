using CityMicroService.DAL.Entities;
using CityMicroService.DAL.RepositoryBase;

namespace CityMicroService.DAL.Repositories;

public class CountryRepository : RepositoryBase<Country>, ICountryRepository
{
    public CountryRepository(ApplicationDbContext appDbContext) : base(appDbContext)
    {

    }
}