using IdentityMicroService.BLL.DAL.Data;
using IdentityMicroService.BLL.Protos;

namespace IdentityMicroService.BLL.Clients.Grpc;

public interface ICityClient
{
    IEnumerable<GrpcCityModel> GetAllCities();
    IEnumerable<GrpcCountryModel> GetAllCountries();
}