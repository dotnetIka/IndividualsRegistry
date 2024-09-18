using FluentValidation;

namespace IndividualsRegistry.Application.Features.Person.Queries.GetPersonList;
public class GetPersonListQueryValidator : AbstractValidator<GetPersonListQuery>
{
    public GetPersonListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0).WithMessage("Page number must be greater than 0.");
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
        RuleFor(x => x.SearchTerm).MaximumLength(100).WithMessage("Search term cannot exceed 100 characters.");
    }
}