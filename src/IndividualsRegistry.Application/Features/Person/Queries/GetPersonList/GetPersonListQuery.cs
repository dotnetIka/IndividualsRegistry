using IndividualsRegistry.Application.Features.Person.Queries.GetPerson.Dtos;
using IndividualsRegistry.Shared.Library;
using IndividualsRegistry.Shared.Mediator;

namespace IndividualsRegistry.Application.Features.Person.Queries.GetPersonList;
public record GetPersonListQuery(
    string SearchTerm,
    int Page = 1,
    int PageSize = 10
) : IQuery<PagedList<GetPersonDto>>;
