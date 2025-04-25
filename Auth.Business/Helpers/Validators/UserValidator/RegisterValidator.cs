
using Auth.Business.Helpers.DTOs.UserDto;
using Auth.Core.Entities.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Auth.Business.Helpers.Validators.UserValidator
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        private readonly UserManager<User> _userManager;

        public RegisterValidator(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public RegisterValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ad hissesi bos ola bilmez")
                .Matches(@"[A-Za-z]").WithMessage("Adiniz yalniz kicik ve boyuk herflerden ibaret olsun");
            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Soyad hissesi bos ola bilmez")
                .Matches(@"[a-zA-Z]").WithMessage("Soyad yalniz kicik ve boyuk herflerden ibaret olmalidir");
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Istifadeci adi bos ola bilmez");
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("duzgun email formati daxil edin");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Sifre hissesi bos ola bilmez")
                .Matches(@"[a-z]").WithMessage("Sifrede en azi 1 eded kicik herf olmalidir")
                .Matches(@"[A-Z]").WithMessage("Sifrede en azi 1 eded boyuk herf olmalidir")
                .Matches(@"[0-9]").WithMessage("Sifrede en azi 1 eded reqem olmalidir");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Sifreler eyni deyil");

        }
    }
}
