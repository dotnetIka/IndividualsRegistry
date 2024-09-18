using IndividualsRegistry.Domain.Interfaces;
using IndividualsRegistry.Shared.Library;
using MediatR;
using Serilog;

namespace IndividualsRegistry.Application.Features.Person.Commands.RemoveRelatedPerson;

public sealed class RemoveRelatedPersonCommandHandler : IRequestHandler<RemoveRelatedPersonCommand, Result<bool>>
{
    private readonly IUnitOfWork _uow;

    public RemoveRelatedPersonCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<bool>> Handle(RemoveRelatedPersonCommand request, CancellationToken cancellationToken)
    {
        Log.Information("Starting to process RemoveRelatedPersonCommand for RelationshipId: {RelationshipId}", request.RelationshipId);

        try
        {
            await _uow.BeginTransactionAsync();
            Log.Debug("Transaction begun");

            var relationships = await _uow.Relationships.FindAsync(r => r.Id == request.RelationshipId);

            if (relationships == null || !relationships.Any())
            {
                await _uow.RollbackTransactionAsync();
                Log.Warning("Relationship not found for removal. RelationshipId: {RelationshipId}", request.RelationshipId);
                return Result.Failure<bool>(new Error("Relationship.NotFound", "The specified relationship was not found.", ErrorTypeEnum.InternalServerError));
            }

            Log.Debug("Found {Count} relationships for RelationshipId: {RelationshipId}", relationships.Count(), request.RelationshipId);

            // Remove the relationship(s)
            foreach (var rel in relationships)
            {
                rel.SoftDelete();
                _uow.Relationships.Update(rel);
                Log.Debug("Soft deleted relationship: {RelationshipId}", rel.Id);
            }

            await _uow.CommitTransactionAsync();
            Log.Information("RemoveRelatedPersonCommand processed successfully. {Count} relationships removed for RelationshipId: {RelationshipId}", relationships.Count(), request.RelationshipId);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while processing RemoveRelatedPersonCommand for RelationshipId: {RelationshipId}, Error message {Message}", request.RelationshipId, ex.Message);
            await _uow.RollbackTransactionAsync();
            Log.Debug("Transaction rolled back");

            return Result.Failure<bool>(new Error("RemoveRelatedPerson.Failed", ex.Message, ErrorTypeEnum.InternalServerError));
        }
    }
}