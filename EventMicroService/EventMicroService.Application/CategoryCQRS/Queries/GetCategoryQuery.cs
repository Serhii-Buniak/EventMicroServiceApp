using AutoMapper;
using EventMicroService.Application.Common.Dtos;
using EventMicroService.Application.Common.Exceptions;
using EventMicroService.Application.Common.Interfaces;
using EventMicroService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventMicroService.Application.CategoryCQRS.Queries;

public class GetCategoryQuery : IRequest<CategoryReadDto>
{
    public GetCategoryQuery(long id)
    {
        Id = id;
    }

    public long Id { get; set; }

    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryReadDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCategoryQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryReadDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            Category? category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category is null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            return _mapper.Map<CategoryReadDto>(category);
        }
    }
}
