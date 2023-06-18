using EventMicroService.Domain.Common;

namespace EventMicroService.Domain.Entities;

public class City : IdentifiedEntity
{
    public string Name { get; set; } = null!;
    public long CountryId { get; set; }
}
