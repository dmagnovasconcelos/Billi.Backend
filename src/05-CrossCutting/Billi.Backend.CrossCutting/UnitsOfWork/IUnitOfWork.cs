using Billi.Backend.CrossCutting.Entities;
using Billi.Backend.CrossCutting.Repositories;

namespace Billi.Backend.CrossCutting.UnitsOfWork
{
    public interface IUnitOfWork<T> : IDisposable where T : BaseEntity
    {
        IGenericCommandRepository<T> Repository { get; }

        Task Start();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
