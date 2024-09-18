using IndividualsRegistry.Domain.Entities;
using System.Linq.Expressions;

namespace IndividualsRegistry.Domain.Interfaces;
public interface IPersonRepository : IGenericRepository<Person>
{
    Task<Person> GetPersonWithDetailsAsync(int id);
    Task<IEnumerable<Person>> GetPersonsWithDetailsAsync(Expression<Func<Person, bool>> predicate);
}