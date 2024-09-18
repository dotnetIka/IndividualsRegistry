using FluentValidation;
using IndividualsRegistry.Application.Features.Person.Shared.Validators;
using System.Text.RegularExpressions;

namespace IndividualsRegistry.Application.Features.Person.Commands.UpdatePerson;
public class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid Person ID.");

        RuleFor(x => x.FirstName)
           .NotEmpty().WithMessage("First name is required.")
           .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.")
           .Matches(@"^[ა-ჰa-zA-Z]+$").WithMessage("First name must contain only Georgian or Latin alphabet letters.")
           .Must(BeConsistentAlphabet).WithMessage("First name must not mix Georgian and Latin alphabets.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.")
            .Matches(@"^[ა-ჰa-zA-Z]+$").WithMessage("Last name must contain only Georgian or Latin alphabet letters.")
            .Must(BeConsistentAlphabet).WithMessage("Last name must not mix Georgian and Latin alphabets.");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required.")
            .Must(x => x == "ქალი" || x == "კაცი").WithMessage("Gender must be either 'ქალი' or 'კაცი'.");

        RuleFor(x => x.PersonalNumber)
            .NotEmpty().WithMessage("Personal number is required.")
            .Length(11).WithMessage("Personal number must be exactly 11 digits.")
            .Matches(@"^\d{11}$").WithMessage("Personal number must contain only digits.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThan(DateTime.Now.AddYears(-18)).WithMessage("Person must be at least 18 years old.");

        RuleForEach(x => x.PhoneNumbers).SetValidator(new PhoneNumberDtoValidator());
    }

    private bool BeConsistentAlphabet(string name)
    {
        return Regex.IsMatch(name, @"^[ა-ჰ]+$") || Regex.IsMatch(name, @"^[a-zA-Z]+$");
    }
}