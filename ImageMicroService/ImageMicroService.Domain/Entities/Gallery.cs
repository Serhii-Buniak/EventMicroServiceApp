using ImageMicroService.Domain.Common;

namespace ImageMicroService.Domain.Entities;

public class Gallery : BaseEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Image> Images { get; set; } = new List<Image>();
}