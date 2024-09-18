using IndividualsRegistry.Shared.Mediator;

namespace IndividualsRegistry.Application.Features.Person.Commands.AddRelatedPerson;
public sealed record AddRelatedPersonCommand(
    int PersonId,
    int RelatedPersonId,
    string RelationType
) : ICommand<bool>;
