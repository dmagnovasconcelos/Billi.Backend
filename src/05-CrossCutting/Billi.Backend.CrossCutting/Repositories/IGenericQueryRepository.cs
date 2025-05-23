using Billi.Backend.CrossCutting.Entities;
using System.Linq.Expressions;

namespace Billi.Backend.CrossCutting.Repositories
{
    public interface IGenericQueryRepository<T> where T : BaseEntity
    {
        Task<T> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>> include = null, CancellationToken cancellationToken = default);

        Task<T> GetAsync(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    }
}
