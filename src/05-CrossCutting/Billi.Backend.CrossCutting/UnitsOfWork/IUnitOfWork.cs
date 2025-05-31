namespace Billi.Backend.CrossCutting.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task Start();

        Task CommitAsync();

        Task RollbackAsync();
    }
}