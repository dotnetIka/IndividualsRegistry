using FluentValidation;

namespace IndividualsRegistry.Application.Features.Person.Commands.DeletePerson;
public class DeletePersonCommandValidator : AbstractValidator<DeletePersonCommand>
{
    public DeletePersonCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid Person ID.");
    }
}