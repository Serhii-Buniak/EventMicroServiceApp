using CityMicroService.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityMicroService.DAL.RepositoryWrapper;

public interface IRepositoryWrapper
{
    ICityRepository CityRepository { get; }
    ICountryRepository CountryRepository { get; }
    int Save();
    Task<int> SaveAsync();
}
