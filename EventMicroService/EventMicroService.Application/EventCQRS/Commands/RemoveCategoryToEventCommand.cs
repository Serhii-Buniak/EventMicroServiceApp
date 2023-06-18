using EventMicroService.Application.Common.Exceptions;
using EventMicroService.Application.Common.Interfaces;
using EventMicroService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventMicroService.Application.EventCQRS.Commands;

public class RemoveCategoryToEventCommand : IRequest
{
    public RemoveCategoryToEventCommand(long eventId, long categoryId)
    {
        EventId = eventId;
        CategoryId = categoryId;
    }

    public long EventId { get; set; }
    public long CategoryId { get; set; }

    public class RemoveCategoryToEventCommandHandler : IRequestHandler<RemoveCategoryToEventCommand>
    {
        private readonly IApplicationDbContext _context;

        public RemoveCategoryToEventCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveCategoryToEventCommand request, CancellationToken cancellationToken)
        {
            Event? @event = await _context.Events
                .Include(c => c.Categories)
                .FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);

            if (@event is null)
            {
                throw new NotFoundException(nameof(Event), request.EventId);
            }

            Category? category = await _context.Categories.FirstOrDefaultAsync(e => e.Id == request.CategoryId, cancellationToken);
            if (category is null)
            {
                throw new NotFoundException(nameof(Category), request.CategoryId);
            }

            @event.Categories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
