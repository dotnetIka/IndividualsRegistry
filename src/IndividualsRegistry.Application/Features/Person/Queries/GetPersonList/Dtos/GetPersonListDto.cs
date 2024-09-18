namespace IndividualsRegistry.Application.Features.Person.Queries.GetPersonList.Dtos;
public sealed class GetPersonListDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonalNumber { get; set; }
}