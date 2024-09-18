using IndividualsRegistry.Application.Features.Person.Shared.Dtos;
using IndividualsRegistry.Shared.Mediator;

namespace IndividualsRegistry.Application.Features.Person.Commands.UpdatePerson;
public sealed record UpdatePersonCommand(
    int Id,
    string FirstName,
    string LastName,
    string Gender,
    string PersonalNumber,
    DateTime DateOfBirth,
    List<PhoneNumberDto> PhoneNumbers
) : ICommand<bool>;