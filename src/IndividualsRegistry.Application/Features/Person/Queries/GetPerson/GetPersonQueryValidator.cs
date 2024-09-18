using FluentValidation;

namespace IndividualsRegistry.Application.Features.Person.Queries.GetPerson;
public class GetPersonQueryValidator : AbstractValidator<GetPersonQuery>
{
    public GetPersonQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid Person ID.");
    }
}