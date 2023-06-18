using AutoMapper;
using EventMicroService.Application.Common.Dtos;
using EventMicroService.Application.Common.Interfaces;
using EventMicroService.Domain.Entities;
using FluentValidation;
using MediatR;

namespace EventMicroService.Application.CategoryCQRS.Commands;

public class UpdateCategoryCommand : IRequest<CategoryReadDto>
{
    protected long Id { get; set; }
    public string Name { get; set; } = null!;

    public void SetId(long id)
    {
        Id = id;
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryReadDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryReadDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            Category category = new() { Id = request.Id, Name = request.Name };

            _context.Categories.Update(category);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryReadDto>(category);
        }
    }

    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCategoryCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(c => c.Name)
                .NotNull()
                .NotEmpty()
                .Must(IsUniqueName);
        }

        private bool IsUniqueName(UpdateCategoryCommand image, string value)
        {
            return !_context.Categories.Any(x => image.Name == x.Name);
        }
    }
}