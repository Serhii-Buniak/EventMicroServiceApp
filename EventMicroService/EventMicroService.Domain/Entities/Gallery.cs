using EventMicroService.Domain.Common;

namespace EventMicroService.Domain.Entities;

public class Gallery : IdentifiedEntity
{
    public string Name { get; set; } = null!;
}
