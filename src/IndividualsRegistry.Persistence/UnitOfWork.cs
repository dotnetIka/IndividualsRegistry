using IndividualsRegistry.Domain.Interfaces;
using IndividualsRegistry.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace IndividualsRegistry.Persistence;
public class UnitOfWork : IUnitOfWork
{
    private readonly IndividualsRegistryDbContext _context;
    private IDbContextTransaction _transaction;

    public IPersonRepository Persons { get; }
    public IPhoneNumberRepository PhoneNumbers { get; }
    public IPersonRelationshipRepository Relationships { get; }

    public UnitOfWork(
        IndividualsRegistryDbContext context,
        IPersonRepository personRepository,
        IPhoneNumberRepository phoneNumberRepository,
        IPersonRelationshipRepository personRelationshipRepository)
    {
        _context = context;
        Persons = personRepository;
        PhoneNumbers = phoneNumberRepository;
        Relationships = personRelationshipRepository;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            await _transaction?.RollbackAsync();
        }
        finally
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}