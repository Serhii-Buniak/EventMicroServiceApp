using FluentValidation;

namespace CityMicroService.BLL.DTOs;

public class CountryRequestDTOValidation : AbstractValidator<CountryRequestDTO>
{
    public CountryRequestDTOValidation()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .NotNull()
            .NotEmpty();
    }
}