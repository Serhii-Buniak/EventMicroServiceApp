using AutoMapper;
using EventMicroService.Application.Common.Dtos;
using EventMicroService.Application.Common.Exceptions;
using EventMicroService.Application.Common.Interfaces;
using EventMicroService.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventMicroService.Application.EventCQRS.Commands;

public class UpdateEventCommand : IRequest<EventReadDto>
{
    protected long Id { get; set; }
    public string Name { get; set; } = null!;
    public long CityId { get; set; }

    public void SetId(long id)
    {
        Id = id;
    }

    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, EventReadDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateEventCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EventReadDto> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            City? city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == request.CityId);

            if (city == null)
            {
                throw new NotFoundException(nameof(City), request.CityId);
            }

            Event @event = new() { Id = request.Id, Name = request.Name, City = city, CityId = request.CityId };

            _context.Events.Update(@event);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EventReadDto>(@event);
        }
    }

    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventCommandValidator(IApplicationDbContext context)
        {
            RuleFor(c => c.Name)
                .NotNull()
                .NotEmpty();
        }
    }
}
