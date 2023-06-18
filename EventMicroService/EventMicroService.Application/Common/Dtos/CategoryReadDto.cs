using EventMicroService.Application.Common.Mappings;
using EventMicroService.Domain.Entities;

namespace EventMicroService.Application.Common.Dtos;

public class CategoryReadDto : IMapFrom<Category>
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
}
