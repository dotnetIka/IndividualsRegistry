using FluentValidation;
using IndividualsRegistry.Application.Features.Person.Shared.Dtos;

namespace IndividualsRegistry.Application.Features.Person.Shared.Validators;
public class PhoneNumberDtoValidator : AbstractValidator<PhoneNumberDto>
{
    public PhoneNumberDtoValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Phone type is required.")
            .Must(x => new[] { "მობილური", "ოფისის", "სახლის" }.Contains(x))
            .WithMessage("Phone type must be either 'მობილური', 'ოფისის', or 'სახლის'.");

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Phone number is required.")
            .Length(4, 50).WithMessage("Phone number must be between 4 and 50 characters.");
    }
}
