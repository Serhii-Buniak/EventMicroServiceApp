using FluentValidation;
using IdentityMicroService.BLL.DAL.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace IdentityMicroService.BLL.Dtos;

public class RegisterDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public IFormFile? Image { get; set; } = null!;
    public long? CityId { get; set; }

    public class RegisterDtoValidation : AbstractValidator<RegisterDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterDtoValidation(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.FirstName)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Username)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .Must(IsUniqueEmail).WithMessage(model => $"Email {model.Email} is already registered.");

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty();
        }

        private bool IsUniqueEmail(RegisterDto model, string value)
        {
            Console.WriteLine(value);
            value = value.ToUpper();
            return !_userManager.Users.Any(u => u.NormalizedEmail == value);
        }
    }
}