using IndividualsRegistry.Domain.Interfaces;
using IndividualsRegistry.Shared.Library;
using IndividualsRegistry.Shared.Mediator;
using Serilog;

namespace IndividualsRegistry.Application.Features.Person.Commands.DeletePerson;

public sealed class DeletePersonCommandHandler : ICommandHandler<DeletePersonCommand>
{
    private readonly IUnitOfWork _uow;

    public DeletePersonCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        Log.Information("Starting to process DeletePersonCommand for PersonId: {PersonId}", request.Id);

        try
        {
            await _uow.BeginTransactionAsync();
            Log.Debug("Transaction begun");

            var person = await _uow.Persons.GetByIdAsync(request.Id);
            if (person == null)
            {
                await _uow.RollbackTransactionAsync();
                Log.Warning("Person not found for deletion. PersonId: {PersonId}", request.Id);
                return Result.Failure(new Error("Person.NotFound", $"Person with ID {request.Id} was not found.", ErrorTypeEnum.InternalServerError));
            }

            Log.Debug("Person found for deletion. PersonId: {PersonId}", request.Id);

            // Implement soft delete
            person.SoftDelete();
            _uow.Persons.Update(person);
            Log.Debug("Person soft deleted. PersonId: {PersonId}", request.Id);

            // Soft delete related phone numbers
            var phoneNumbers = await _uow.PhoneNumbers.FindAsync(pn => pn.PersonId == request.Id);
            foreach (var phoneNumber in phoneNumbers)
            {
                phoneNumber.SoftDelete();
                _uow.PhoneNumbers.Update(phoneNumber);
            }
            Log.Debug("Soft deleted {Count} related phone numbers for PersonId: {PersonId}", phoneNumbers.Count(), request.Id);

            // Soft delete related relationships
            var relationships = await _uow.Relationships.FindAsync(r => r.PersonId == request.Id || r.RelatedPersonId == request.Id);
            foreach (var relationship in relationships)
            {
                relationship.SoftDelete();
                _uow.Relationships.Update(relationship);
            }
            Log.Debug("Soft deleted {Count} related relationships for PersonId: {PersonId}", relationships.Count(), request.Id);

            await _uow.CommitTransactionAsync();
            Log.Information("DeletePersonCommand processed successfully. PersonId: {PersonId}", request.Id);

            return Result.Success();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while processing DeletePersonCommand for PersonId: {PersonId}, Error message {Message}", request.Id, ex.Message);
            await _uow.RollbackTransactionAsync();
            Log.Debug("Transaction rolled back");

            return Result.Failure(new Error("Person.DeleteFailed", ex.Message, ErrorTypeEnum.InternalServerError));
        }
    }
}