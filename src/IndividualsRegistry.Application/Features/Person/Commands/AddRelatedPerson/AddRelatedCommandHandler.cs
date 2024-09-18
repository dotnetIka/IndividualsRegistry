using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Domain.Interfaces;
using IndividualsRegistry.Shared.Library;
using IndividualsRegistry.Shared.Mediator;
using Serilog;

namespace IndividualsRegistry.Application.Features.Person.Commands.AddRelatedPerson;

public class AddRelatedCommandHandler : ICommandHandler<AddRelatedPersonCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public AddRelatedCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<bool>> Handle(AddRelatedPersonCommand request, CancellationToken cancellationToken)
    {
        Log.Information("Starting to process AddRelatedPersonCommand {@Request}", request);

        try
        {
            await _uow.BeginTransactionAsync();
            Log.Debug("Transaction begun");

            var person = await _uow.Persons.GetByIdAsync(request.PersonId);
            var relatedPerson = await _uow.Persons.GetByIdAsync(request.RelatedPersonId);

            if (person == null || relatedPerson == null)
            {
                await _uow.RollbackTransactionAsync();
                Log.Warning("One or both persons were not found. PersonId: {PersonId}, RelatedPersonId: {RelatedPersonId}", request.PersonId, request.RelatedPersonId);
                return Result.Failure<bool>(new Error("Person.NotFound", "One or both persons were not found.", ErrorTypeEnum.BadRequest));
            }

            Log.Debug("Persons found. PersonId: {PersonId}, RelatedPersonId: {RelatedPersonId}", request.PersonId, request.RelatedPersonId);

            var relationship = new PersonRelationship(request.PersonId, request.RelatedPersonId, request.RelationType);
            await _uow.Relationships.AddAsync(relationship);
            Log.Debug("Relationship added: {@Relationship}", relationship);

            await _uow.CommitTransactionAsync();
            Log.Information("AddRelatedPersonCommand processed successfully. Relationship added between PersonId: {PersonId} and RelatedPersonId: {RelatedPersonId}", request.PersonId, request.RelatedPersonId);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while processing AddRelatedPersonCommand, Error message {Message}", ex.Message);
            await _uow.RollbackTransactionAsync();
            Log.Debug("Transaction rolled back");

            return Result.Failure<bool>(new Error("Relationship.AddFailed", ex.Message, ErrorTypeEnum.InternalServerError));
        }
    }
}