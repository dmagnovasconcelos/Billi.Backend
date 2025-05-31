using Billi.Backend.CrossCutting.Contexts;
using Billi.Backend.CrossCutting.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Billi.Backend.CrossCutting.Repositories
{
    public abstract class GenericQueryRepository<T>(BaseDbContext context) : IGenericQueryRepository<T> where T : BaseEntity
    {
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
        {
            return await _dbSet.AnyAsync(filter, cancellationToken);
        }

        public async Task<T> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync([id], cancellationToken);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>> include = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = _dbSet.AsQueryable();

            if (include is not null)
                query = include(query);

            return await query.FirstOrDefaultAsync(filter, cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
        {
            return await _dbSet.Where(filter).ToListAsync(cancellationToken);
        }
    }
}