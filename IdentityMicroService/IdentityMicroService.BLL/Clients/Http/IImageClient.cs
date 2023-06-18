using IdentityMicroService.BLL.DAL.Data;
using IdentityMicroService.BLL.Dtos;
using Microsoft.AspNetCore.Http;

namespace IdentityMicroService.BLL.Clients.Http;

public interface IImageClient
{
    Task<Image?> CreateImage(IFormFile image);
}
