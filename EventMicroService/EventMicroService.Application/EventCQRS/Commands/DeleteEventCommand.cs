using AutoMapper;
using EventMicroService.Application.Common.Exceptions;
using EventMicroService.Application.Common.Interfaces;
using EventMicroService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventMicroService.Application.EventCQRS.Commands;

public class DeleteEventCommand : IRequest
{
    public DeleteEventCommand(long id)
    {
        Id = id;
    }

    public long Id { get; set; }

    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteEventCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            Event? @event = await _context.Events.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (@event is null)
            {
                throw new NotFoundException(nameof(Event), request.Id);
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
