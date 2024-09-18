using FluentValidation;

namespace IndividualsRegistry.Application.Features.Person.Commands.AddRelatedPerson;
public class AddRelatedPersonCommandValidator : AbstractValidator<AddRelatedPersonCommand>
{
    public AddRelatedPersonCommandValidator()
    {
        RuleFor(x => x.PersonId).GreaterThan(0).WithMessage("Invalid Person ID.");
        RuleFor(x => x.RelatedPersonId).GreaterThan(0).WithMessage("Invalid Related Person ID.");
        RuleFor(x => x.RelationType)
            .NotEmpty().WithMessage("Relation type is required.")
            .Must(x => new[] { "კოლეგა", "ნაცნობი", "ნათესავი", "სხვა" }.Contains(x))
            .WithMessage("Relation type must be either 'კოლეგა', 'ნაცნობი', 'ნათესავი', or 'სხვა'.");
    }
}