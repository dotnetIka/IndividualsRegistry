using IndividualsRegistry.Shared.Mediator;

namespace IndividualsRegistry.Application.Features.Person.Commands.RemoveRelatedPerson;
public sealed record RemoveRelatedPersonCommand(
    int RelationshipId
) : ICommand<bool>;