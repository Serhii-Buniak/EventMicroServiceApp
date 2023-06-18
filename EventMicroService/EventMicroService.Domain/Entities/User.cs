using EventMicroService.Domain.Common;

namespace EventMicroService.Domain.Entities;

public class User : IdentifiedEntity<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public long? CityId { get; set; }
    public City? City { get; set; }
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
