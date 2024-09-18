using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Domain.Interfaces;
using IndividualsRegistry.Shared.Library;
using IndividualsRegistry.Shared.Mediator;
using Serilog;

namespace IndividualsRegistry.Application.Features.Person.Commands.UpdatePerson;

public class UpdatePersonCommandHandler : ICommandHandler<UpdatePersonCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public UpdatePersonCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<bool>> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        Log.Information("Starting to process UpdatePersonCommand for PersonId: {PersonId}", request.Id);

        try
        {
            await _uow.BeginTransactionAsync();
            Log.Debug("Transaction begun");

            var person = await _uow.Persons.GetByIdAsync(request.Id);
            if (person == null)
            {
                await _uow.RollbackTransactionAsync();
                Log.Warning("Person not found for update. PersonId: {PersonId}", request.Id);
                return Result.Failure<bool>(new Error("Person.NotFound", $"Person with ID {request.Id} was not found.", ErrorTypeEnum.InternalServerError));
            }

            Log.Debug("Person found for update. PersonId: {PersonId}", request.Id);

            person.UpdateDetails(request.FirstName, request.LastName, request.Gender, request.PersonalNumber, request.DateOfBirth);
            _uow.Persons.Update(person);
            Log.Debug("Person details updated. PersonId: {PersonId}", request.Id);

            // Remove existing phone numbers
            var existingPhoneNumbers = await _uow.PhoneNumbers.FindAsync(pn => pn.PersonId == request.Id);
            foreach (var phoneNumber in existingPhoneNumbers)
            {
                _uow.PhoneNumbers.SoftDelete(phoneNumber);
            }
            Log.Debug("Soft deleted {Count} existing phone numbers for PersonId: {PersonId}", existingPhoneNumbers.Count(), request.Id);

            // Add new phone numbers
            foreach (var phoneNumberDto in request.PhoneNumbers)
            {
                var phoneNumber = new PhoneNumber(phoneNumberDto.Type, phoneNumberDto.Number, person.Id);
                await _uow.PhoneNumbers.AddAsync(phoneNumber);
            }
            Log.Debug("Added {Count} new phone numbers for PersonId: {PersonId}", request.PhoneNumbers.Count(), request.Id);

            await _uow.CommitTransactionAsync();
            Log.Information("UpdatePersonCommand processed successfully. PersonId: {PersonId}", request.Id);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while processing UpdatePersonCommand for PersonId: {PersonId}. Error message: {ErrorMessage}", request.Id, ex.Message);
            await _uow.RollbackTransactionAsync();
            Log.Debug("Transaction rolled back");

            return Result.Failure<bool>(new Error("Person.UpdateFailed", ex.Message, ErrorTypeEnum.InternalServerError));
        }
    }
}