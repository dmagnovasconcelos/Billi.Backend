using Billi.Backend.CrossCutting.Entities;

namespace Billi.Backend.CrossCutting.Repositories
{
    public interface IGenericCommandRepository<T> : IGenericQueryRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

        void Update(T entity);

        void UpdateRange(IEnumerable<T> entities);
    }
}