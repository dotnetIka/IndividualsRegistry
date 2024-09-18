using IndividualsRegistry.Application.Features.Person.Shared.Dtos;

namespace IndividualsRegistry.Application.Features.Person.Queries.GetPerson.Dtos;
public sealed class GetPersonDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public string PersonalNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public List<PhoneNumberDto> PhoneNumbers { get; set; }
    public List<RelatedPersonDto> RelatedPersons { get; set; }
}