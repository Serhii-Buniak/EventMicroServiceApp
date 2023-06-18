using AutoMapper;
using EventMicroService.Application.CategoryCQRS.Queries;
using EventMicroService.Application.Common.Dtos;
using EventMicroService.Application.Common.Exceptions;
using EventMicroService.Application.Common.Interfaces;
using EventMicroService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventMicroService.Application.EventCQRS.Queries;

public class GetEventQuery : IRequest<EventReadDto>
{
    public GetEventQuery(long id)
    {
        Id = id;
    }

    public long Id { get; set; }

    public class GetEventQueryHandler : IRequestHandler<GetEventQuery, EventReadDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetEventQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EventReadDto> Handle(GetEventQuery request, CancellationToken cancellationToken)
        {
            Event? @event = await _context.Events
                .Include(e => e.User)
                .Include(e => e.Gallery)
                .Include(e => e.Categories)
                .Include(e => e.City)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (@event is null)
            {
                throw new NotFoundException(nameof(Event), request.Id);
            }

            return _mapper.Map<EventReadDto>(@event);
        }
    }
}
