using Application.Abstracts.Repositories;
using Application.Dtos.City;
using FluentValidation;

namespace Application.Validations.CityValidation;

public class UpdateCityValidation : AbstractValidator<UpdateCityRequest>
{
    public UpdateCityValidation(ICityRepository cityRepository)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id düzgün deyil");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("City name boş ola bilməz")
            .MaximumLength(100).WithMessage("City name maksimum 100 simvol ola bilər")
            .MustAsync(async (dto, name, cancellation) =>
                !await cityRepository.ExistsByNameAsync( name))
            .WithMessage("Bu adda başqa bir city artıq mövcuddur");
    }
}