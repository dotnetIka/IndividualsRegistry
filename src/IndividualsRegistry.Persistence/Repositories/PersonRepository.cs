using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Domain.Interfaces;
using IndividualsRegistry.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IndividualsRegistry.Persistence.Repositories;
public class PersonRepository : GenericRepository<Person>, IPersonRepository
{
    public PersonRepository(IndividualsRegistryDbContext context) : base(context)
    {
    }

    public async Task<Person?> GetPersonWithDetailsAsync(int id)
    {
        return await _context.Persons
            .Include(p => p.PhoneNumbers)
            .Include(p => p.Relationships)
                .ThenInclude(r => r.RelatedPerson)
            .FirstOrDefaultAsync(p => p.Id == id);
    }


    public async Task<IEnumerable<Person>> GetPersonsWithDetailsAsync(Expression<Func<Person, bool>> predicate)
    {
        return await _context.Persons
            .Where(predicate)
            .Include(p => p.PhoneNumbers)
            .Include(p => p.Relationships)
                .ThenInclude(r => r.RelatedPerson)
            .ToListAsync();
    }
}
