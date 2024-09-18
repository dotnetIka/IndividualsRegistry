using IndividualsRegistry.Application.Features.Person.Queries.GetPerson.Dtos;
using IndividualsRegistry.Shared.Mediator;

namespace IndividualsRegistry.Application.Features.Person.Queries.GetPerson;

public sealed record GetPersonQuery(int Id) : IQuery<GetPersonDto>;
