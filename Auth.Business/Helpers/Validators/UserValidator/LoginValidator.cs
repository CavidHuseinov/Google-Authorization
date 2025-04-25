using Auth.Business.Helpers.DTOs.UserDto;
using FluentValidation;

namespace Auth.Business.Helpers.Validators.UserValidator
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UsernameOrEmail).NotEmpty().WithMessage("Istifadeci adi ve ya sifre dogru deyil");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Istifadeci adi ve ya sifre dogru deyil");
        }
    }
}
