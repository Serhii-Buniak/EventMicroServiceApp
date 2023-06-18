using ImageMicroService.Application.Common.Models;

namespace ImageMicroService.Application.Common.Interfaces
{
    public interface IImagePublisher
    {
        void DeleteEvent(ImageDto image);
    }
}