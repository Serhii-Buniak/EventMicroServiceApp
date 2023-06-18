using ImageMicroService.Application.Common.Mappings;
using ImageMicroService.Domain.Entities;

namespace ImageMicroService.Application.Common.Models;

public class ImageDto : IMapFrom<Image>
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long? GalleryId { get; set; }
}