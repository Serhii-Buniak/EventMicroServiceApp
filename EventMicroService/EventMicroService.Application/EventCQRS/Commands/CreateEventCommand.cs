using AutoMapper;
using EventMicroService.Application.CategoryCQRS.Commands;
using EventMicroService.Application.Common.Dtos;
using EventMicroService.Application.Common.Exceptions;
using EventMicroService.Application.Common.Interfaces;
using EventMicroService.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace EventMicroService.Application.EventCQRS.Commands;

public class CreateEventCommand : IRequest<EventReadDto>
{
    public string Name { get; set; } = null!;
    public long CityId { get; set; }
    public Guid UserId { get; set; }
    public string GalleryName { get; set; } = null!;

    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventReadDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IGalleryClient _client;

        public CreateEventCommandHandler(IApplicationDbContext context, IMapper mapper, IGalleryClient client)
        {
            _context = context;
            _mapper = mapper;
            _client = client;
        }

        public async Task<EventReadDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            City? city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == request.CityId);

            if (city == null)
            {
                throw new NotFoundException(nameof(City), request.CityId);
            }

            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.UserId);
            }

            Gallery? gallery = await _client.CreateGalleryAsync(new CreateGalleryDto { Name = request.Name });

            if (gallery == null)
            {
                throw new ClientException(nameof(IGalleryClient));
            }

            Event @event = new() { Name = request.Name, City = city, User = user, Gallery = gallery };

            await _context.Events.AddAsync(@event, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EventReadDto>(@event);
        }
    }

    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateEventCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(c => c.Name)
                .NotNull()
                .NotEmpty()
                .Must(IsUniqueName);
        }

        private bool IsUniqueName(CreateEventCommand image, string value)
        {
            return !_context.Events.Any(x => image.Name == x.Name);
        }
    }
}
