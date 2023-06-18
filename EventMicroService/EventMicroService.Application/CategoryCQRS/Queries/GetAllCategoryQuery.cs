using AutoMapper;
using EventMicroService.Application.Common.Dtos;
using EventMicroService.Application.Common.Interfaces;
using EventMicroService.Application.Common.Mappings;
using EventMicroService.Domain.Entities;
using MediatR;

namespace EventMicroService.Application.CategoryCQRS.Queries;

public class GetAllCategoryQuery : IRequest<IEnumerable<CategoryReadDto>>
{

    public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, IEnumerable<CategoryReadDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllCategoryQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryReadDto>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _context.Categories.ProjectToListAsync<CategoryReadDto>(_mapper.ConfigurationProvider);
        }
    }
}
