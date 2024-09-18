using FluentValidation;

namespace IndividualsRegistry.Application.Features.Person.Commands.RemoveRelatedPerson;

public class RemoveRelatedPersonCommandValidator : AbstractValidator<RemoveRelatedPersonCommand>
{
    public RemoveRelatedPersonCommandValidator()
    {
        RuleFor(x => x.RelationshipId).GreaterThan(0).WithMessage("Invalid Relationship ID.");
    }
}
