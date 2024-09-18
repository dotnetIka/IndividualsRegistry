using IndividualsRegistry.Domain.Interfaces;
using IndividualsRegistry.Infrastructure.Persistence;
using IndividualsRegistry.Shared.Library;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IndividualsRegistry.Persistence.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : Entity
{
    protected readonly IndividualsRegistryDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(IndividualsRegistryDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id && e.DeletedAt == null);
    }

    public virtual async Task<int> AddAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public virtual void Update(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public virtual void SoftDelete(T entity)
    {
        entity.DeletedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> expression = null)
    {
        IQueryable<T> query = _dbSet.Where(e => e.DeletedAt == null);
        if (expression != null)
        {
            query = query.Where(expression);
        }
        return await query.CountAsync();
    }

    public async Task<List<T>> FindAsync(
    Expression<Func<T, bool>> predicate,
    params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>().Where(predicate);

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.ToListAsync();
    }
}
