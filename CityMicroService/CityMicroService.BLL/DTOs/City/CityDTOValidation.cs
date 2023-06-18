using FluentValidation;

namespace CityMicroService.BLL.DTOs;

public class CityDTOValidation : AbstractValidator<CityDTO>
{
    public CityDTOValidation()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Country)
            .NotNull();
    }
}