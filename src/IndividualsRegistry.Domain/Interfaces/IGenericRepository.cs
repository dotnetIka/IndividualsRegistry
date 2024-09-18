using IndividualsRegistry.Shared.Library;
using System.Linq.Expressions;

namespace IndividualsRegistry.Domain.Interfaces;
public interface IGenericRepository<T> where T : Entity
{
    Task<T> GetByIdAsync(int id);
    Task<int> AddAsync(T entity);
    void Update(T entity);
    void SoftDelete(T entity);
    Task<int> CountAsync(Expression<Func<T, bool>> expression = null);
    Task<List<T>> FindAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includeProperties);
}