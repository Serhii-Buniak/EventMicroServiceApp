using EventMicroService.Application.Common.Dtos;
using EventMicroService.Domain.Entities;

namespace EventMicroService.Application.Common.Interfaces
{
    public interface IGalleryClient
    {
        Task<Gallery?> CreateGalleryAsync(CreateGalleryDto createGallery);
    }
}