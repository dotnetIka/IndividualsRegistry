namespace IndividualsRegistry.Domain.Interfaces;
public interface IUnitOfWork : IDisposable
{
    IPersonRepository Persons { get; }
    IPhoneNumberRepository PhoneNumbers { get; }
    IPersonRelationshipRepository Relationships { get; }

    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}