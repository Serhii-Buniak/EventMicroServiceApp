using AutoMapper;
using EventMicroService.Application.Common.Dtos;
using EventMicroService.Application.Common.Interfaces;
using EventMicroService.Application.Common.Mappings;
using EventMicroService.Domain.Entities;
using FluentValidation;
using MediatR;

namespace EventMicroService.Application.CategoryCQRS.Commands;

public class CreateCategoryCommand : IRequest<CategoryReadDto>
{
    public string Name { get; set; } = null!;

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryReadDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryReadDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            Category category = new() { Name = request.Name };

            await _context.Categories.AddAsync(category, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryReadDto>(category);
        }
    }

    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateCategoryCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(c => c.Name)
                .NotNull()
                .NotEmpty()
                .Must(IsUniqueName);
        }

        private bool IsUniqueName(CreateCategoryCommand image, string value)
        {
            return !_context.Categories.Any(x => image.Name == x.Name);
        }
    }
}
