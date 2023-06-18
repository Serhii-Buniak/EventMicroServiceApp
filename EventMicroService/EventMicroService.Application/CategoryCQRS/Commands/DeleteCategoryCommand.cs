using AutoMapper;
using EventMicroService.Application.Common.Exceptions;
using EventMicroService.Application.Common.Interfaces;
using EventMicroService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventMicroService.Application.CategoryCQRS.Commands;

public class DeleteCategoryCommand : IRequest
{
    public DeleteCategoryCommand(long id)
    {
        Id = id;
    }

    public long Id { get; set; }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            Category? category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category is null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
