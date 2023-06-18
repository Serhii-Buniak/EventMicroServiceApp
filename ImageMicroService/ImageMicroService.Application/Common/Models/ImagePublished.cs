using ImageMicroService.Application.Common.Mappings;

namespace ImageMicroService.Application.Common.Models;

public class ImagePublished : ModelPublished, IMapFrom<ImageDto>
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long? GalleryId { get; set; }
}
