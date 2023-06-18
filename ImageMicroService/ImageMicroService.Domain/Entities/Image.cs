using ImageMicroService.Domain.Common;

namespace ImageMicroService.Domain.Entities;

public class Image : BaseEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long? GalleryId { get; set; }
    public Gallery? Gallery { get; set; }
}
