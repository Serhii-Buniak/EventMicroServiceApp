using ImageMicroService.Application.Common.Interfaces;
using ImageMicroService.Application.Common.Models;
using AutoMapper;
using ImageMicroService.Domain.Entities;
using FluentValidation;
using MediatR;

namespace ImageMicroService.Application.GalleryActions.Commands;

public class CreateGalleryCommand : IRequest<GalleryDto>
{
    public string Name { get; set; } = null!;

    public class CreateGalleryCommandHandler : IRequestHandler<CreateGalleryCommand, GalleryDto>
    {
        private readonly IApplicationDbContext _dataContext;
        private readonly IMapper _mapper;

        public CreateGalleryCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _dataContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<GalleryDto> Handle(CreateGalleryCommand request, CancellationToken cancellationToken)
        {
            Gallery gallery = new()
            {
                Name = request.Name,
            };

            await _dataContext.Galleries.AddAsync(gallery, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<GalleryDto>(gallery);
        }
    }

    public class CreateGalleryCommandValidator : AbstractValidator<CreateGalleryCommand>
    {
        private readonly IApplicationDbContext _dataContext;

        public CreateGalleryCommandValidator(IApplicationDbContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100)
                .Must(IsUniqueName).WithMessage(x => $"Gallery with '{x.Name}' Name already exist");
        }

        private bool IsUniqueName(CreateGalleryCommand image, string value)
        {
            return !_dataContext.Galleries.Any(x => image.Name == x.Name);
        }
    }
}