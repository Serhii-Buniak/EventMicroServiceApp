using EventMicroService.Application.Common.Mappings;
using EventMicroService.Domain.Entities;

namespace EventMicroService.Application.Common.Dtos;

public class EventReadDto : IMapFrom<Event>
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long CityId { get; set; }
    public Guid UserId { get; set; }
    public long GalleryId { get; set; }
    public ICollection<CategoryReadDto> Categories { get; set; } = null!;
}
