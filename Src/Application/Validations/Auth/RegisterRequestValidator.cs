using Application.Dtos.Auth;
using FluentValidation;

namespace Application.Validations.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName boş ola bilməz")
            .MinimumLength(3).WithMessage("UserName minimum 3 simvol olmalıdır")
            .MaximumLength(50).WithMessage("UserName maksimum 50 simvol ola bilər");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email boş ola bilməz")
            .EmailAddress().WithMessage("Email formatı düzgün deyil")
            .MaximumLength(256).WithMessage("Email maksimum 256 simvol ola bilər");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password boş ola bilməz")
            .MinimumLength(8).WithMessage("Password minimum 8 simvol olmalıdır")
            .MaximumLength(100).WithMessage("Password maksimum 100 simvol ola bilər")
            .Matches("[0-9]").WithMessage("Password ən azı 1 rəqəm içerməlidir")
            .Matches("[A-Z]").WithMessage("Password ən azı 1 böyük hərf içerməlidir")
            .Matches("[a-z]").WithMessage("Password ən azı 1 kiçik hərf içerməlidir")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password ən azı 1 xüsusi simvol içerməlidir");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("FullName boş ola bilməz")
            .MaximumLength(200).WithMessage("FullName maksimum 200 simvol ola bilər");
    }
}

