using IndividualsRegistry.Shared.Mediator;

namespace IndividualsRegistry.Application.Features.Person.Commands.DeletePerson;
public sealed record DeletePersonCommand(int Id) : ICommand;