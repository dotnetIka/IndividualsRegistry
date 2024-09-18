using IndividualsRegistry.Application.Features.Person.Queries.GetPerson.Dtos;
using IndividualsRegistry.Application.Features.Person.Shared.Dtos;
using IndividualsRegistry.Domain.Interfaces;
using IndividualsRegistry.Shared.Library;
using IndividualsRegistry.Shared.Mediator;
using Serilog;
using System.Linq.Expressions;

namespace IndividualsRegistry.Application.Features.Person.Queries.GetPersonList;

public sealed class GetPersonListQueryHandler : IQueryHandler<GetPersonListQuery, PagedList<GetPersonDto>>
{
    private readonly IUnitOfWork _uow;

    public GetPersonListQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<PagedList<GetPersonDto>>> Handle(GetPersonListQuery request, CancellationToken cancellationToken)
    {
        Log.Information("Starting to process GetPersonListQuery. Page: {Page}, PageSize: {PageSize}, SearchTerm: {SearchTerm}",
            request.Page, request.PageSize, request.SearchTerm);

        try
        {
            // Predicate to filter persons based on the search term and deleted flag
            Expression<Func<Domain.Entities.Person, bool>> predicate = p =>
                p.DeletedAt == null && (string.IsNullOrEmpty(request.SearchTerm) ||
                p.FirstName.Contains(request.SearchTerm) ||
                p.LastName.Contains(request.SearchTerm) ||
                p.PersonalNumber.Contains(request.SearchTerm));

            Log.Debug("Applying filter predicate for person list");

            // Get total count of persons matching the predicate
            var totalCount = await _uow.Persons.CountAsync(predicate);
            Log.Debug("Total count of matching persons: {TotalCount}", totalCount);

            // Fetch persons with details using the repository method
            var personsWithDetails = await _uow.Persons.GetPersonsWithDetailsAsync(predicate);
            Log.Debug("Retrieved {Count} persons with details", personsWithDetails.Count());

            // Apply pagination manually
            var pagedPersons = personsWithDetails
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new GetPersonDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Gender = p.Gender,
                    PersonalNumber = p.PersonalNumber,
                    DateOfBirth = p.DateOfBirth,
                    PhoneNumbers = p.PhoneNumbers
                        .Where(pn => pn.DeletedAt == null)
                        .Select(pn => new PhoneNumberDto
                        {
                            Type = pn.Type,
                            Number = pn.Number
                        }).ToList(),
                    RelatedPersons = p.Relationships
                        .Where(r => r.DeletedAt == null && r.RelatedPerson != null && r.RelatedPerson.DeletedAt == null)
                        .Select(r => new RelatedPersonDto
                        {
                            Id = r.RelatedPersonId,
                            FullName = $"{r.RelatedPerson.FirstName} {r.RelatedPerson.LastName}",
                            RelationType = r.RelationType
                        }).ToList()
                })
                .ToList();

            Log.Debug("Applied pagination. Retrieved {Count} persons for current page", pagedPersons.Count);

            // Create a paged list to return
            var pagedList = new PagedList<GetPersonDto>(pagedPersons, totalCount, request.Page, request.PageSize);

            Log.Information("GetPersonListQuery processed successfully. Retrieved {Count} persons out of {TotalCount}",
                pagedPersons.Count, totalCount);

            return Result.Success(pagedList);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while processing GetPersonListQuery. Error message: {ErrorMessage}", ex.Message);
            return Result.Failure<PagedList<GetPersonDto>>(new Error("GetPersonList.Failed", ex.Message, ErrorTypeEnum.InternalServerError));
        }
    }
}