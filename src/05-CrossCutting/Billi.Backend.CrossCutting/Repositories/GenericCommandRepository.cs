using Billi.Backend.CrossCutting.Contexts;
using Billi.Backend.CrossCutting.Entities;
using Microsoft.EntityFrameworkCore;

namespace Billi.Backend.CrossCutting.Repositories
{
    public abstract class GenericCommandRepository<T>(BaseDbContext context) : IGenericCommandRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
           _dbSet.UpdateRange(entities);
        }
    }
}
