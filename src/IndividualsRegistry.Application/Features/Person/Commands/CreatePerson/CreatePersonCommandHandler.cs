using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Domain.Interfaces;
using IndividualsRegistry.Shared.Library;
using IndividualsRegistry.Shared.Mediator;
using Serilog;

namespace IndividualsRegistry.Application.Features.Person.Commands.CreatePerson;

public class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand, int>
{
    private readonly IUnitOfWork _uow;

    public CreatePersonCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<int>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        Log.Information("Starting to process CreatePersonCommand {@Request}", request);

        try
        {
            await _uow.BeginTransactionAsync();
            Log.Debug("Transaction begun");

            var person = new Domain.Entities.Person(request.FirstName,
                                    request.LastName,
                                    request.Gender,
                                    request.PersonalNumber,
                                    request.DateOfBirth);

            var personId = await _uow.Persons.AddAsync(person);
            Log.Debug("Person added with ID {PersonId}", personId);

            // Add new phone numbers
            foreach (var phoneNumberDto in request.PhoneNumbers)
            {
                var phoneNumber = new PhoneNumber(phoneNumberDto.Type, phoneNumberDto.Number, personId);
                await _uow.PhoneNumbers.AddAsync(phoneNumber);
                Log.Debug("Phone number added: {PhoneNumber}", phoneNumber);
            }

            await _uow.CommitTransactionAsync();
            Log.Information("CreatePersonCommand processed successfully. Person ID: {PersonId}", person.Id);

            return Result.Success(person.Id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while processing CreatePersonCommand, Error message", ex.Message);
            await _uow.RollbackTransactionAsync();
            Log.Debug("Transaction rolled back");

            return Result.Failure<int>(new Error("Person.CreateFailed", ex.Message, ErrorTypeEnum.InternalServerError));
        }
    }
}