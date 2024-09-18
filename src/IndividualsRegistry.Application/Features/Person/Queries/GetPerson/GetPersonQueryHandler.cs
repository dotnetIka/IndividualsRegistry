using IndividualsRegistry.Application.Features.Person.Queries.GetPerson.Dtos;
using IndividualsRegistry.Application.Features.Person.Shared.Dtos;
using IndividualsRegistry.Domain.Interfaces;
using IndividualsRegistry.Shared.Library;
using IndividualsRegistry.Shared.Mediator;
using Serilog;

namespace IndividualsRegistry.Application.Features.Person.Queries.GetPerson;

public sealed class GetPersonQueryHandler : IQueryHandler<GetPersonQuery, GetPersonDto>
{
    private readonly IUnitOfWork _uow;

    public GetPersonQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<GetPersonDto>> Handle(GetPersonQuery request, CancellationToken cancellationToken)
    {
        Log.Information("Starting to process GetPersonQuery for PersonId: {PersonId}", request.Id);

        try
        {
            var personEntity = await _uow.Persons.GetPersonWithDetailsAsync(request.Id);

            if (personEntity == null)
            {
                Log.Warning("Person not found. PersonId: {PersonId}", request.Id);
                return Result.Failure<GetPersonDto>(new Error("Person.NotFound", $"Person with ID {request.Id} was not found.", ErrorTypeEnum.BadRequest));
            }

            Log.Debug("Person found. PersonId: {PersonId}", request.Id);

            var dto = new GetPersonDto
            {
                Id = personEntity.Id,
                FirstName = personEntity.FirstName,
                LastName = personEntity.LastName,
                Gender = personEntity.Gender,
                PersonalNumber = personEntity.PersonalNumber,
                DateOfBirth = personEntity.DateOfBirth,
                PhoneNumbers = personEntity.PhoneNumbers.Select(pn => new PhoneNumberDto
                {
                    Type = pn.Type,
                    Number = pn.Number
                }).ToList(),
                RelatedPersons = personEntity.Relationships.Select(r => new RelatedPersonDto
                {
                    Id = r.RelatedPersonId,
                    FullName = $"{r.RelatedPerson.FirstName} {r.RelatedPerson.LastName}",
                    RelationType = r.RelationType
                }).ToList()
            };

            Log.Debug("Person DTO created. PersonId: {PersonId}, PhoneNumbers: {PhoneNumberCount}, RelatedPersons: {RelatedPersonCount}",
                dto.Id, dto.PhoneNumbers.Count, dto.RelatedPersons.Count);

            Log.Information("GetPersonQuery processed successfully for PersonId: {PersonId}", request.Id);

            return Result.Success(dto);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while processing GetPersonQuery for PersonId: {PersonId}. Error message: {ErrorMessage}", request.Id, ex.Message);
            return Result.Failure<GetPersonDto>(new Error("GetPerson.Failed", ex.Message, ErrorTypeEnum.InternalServerError));
        }
    }
}