using ImageMicroService.Application.Common.Mappings;
using ImageMicroService.Domain.Entities;

namespace ImageMicroService.Application.Common.Models;

public class GalleryDto : IMapFrom<Gallery>
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public IEnumerable<ImageDto> Images { get; set; } = Enumerable.Empty<ImageDto>();
}