using EventMicroService.Domain.Common;

namespace EventMicroService.Domain.Entities;

public class Category : IdentifiedEntity
{   
    public string Name { get; set; } = null!;
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
