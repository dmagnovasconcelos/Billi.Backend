using Billi.Backend.CrossCutting.Contexts;
using Billi.Backend.CrossCutting.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Billi.Backend.CrossCutting.Repositories
{
    public abstract class GenericQueryRepository<T>(BaseDbContext context) : IGenericQueryRepository<T> where T : BaseEntity
    {
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
        {
           return await _dbSet.FirstOrDefaultAsync(filter, cancellationToken);  
        }

        public async Task<T> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync(id, cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
        {
            return await _dbSet.Where(filter).ToListAsync(cancellationToken);
        }
    }
}
