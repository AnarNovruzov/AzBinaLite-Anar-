using Application.Abstracts.Repositories;
using Application.Dtos.City;
using FluentValidation;

namespace Application.Validations.CityValidation;

    public class CreateCityValidation : AbstractValidator<CreateCityRequest>
    {
        public CreateCityValidation(ICityRepository cityRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("City name boş ola bilməz")
                .MaximumLength(100).WithMessage("City name maksimum 100 simvol ola bilər")
                .MustAsync(async (name, cancellation) =>
                    !await cityRepository.ExistsByNameAsync(name))
                .WithMessage("Bu adda city artıq mövcuddur");
        }
    }
