using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Billi.Backend.CrossCutting.Contexts
{
    public class BaseDbContext(DbContextOptions<BaseDbContext> options) : DbContext(options)
    {
        private const string _messageExceptionLogicalDelete = "Use logical delete instead of Remove.";
        private const string _messageExceptionSaveChanges = "Use SaveChangesAsync instead of SaveChanges.";

        public override EntityEntry Remove(object entity)
        {
            throw new NotImplementedException(_messageExceptionLogicalDelete);
        }

        public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        {
            throw new NotImplementedException(_messageExceptionLogicalDelete);
        }

        public override void RemoveRange(params object[] entities)
        {
            throw new NotImplementedException(_messageExceptionLogicalDelete);
        }

        public override void RemoveRange(IEnumerable<object> entities)
        {
            throw new NotImplementedException(_messageExceptionLogicalDelete);
        }

        public override int SaveChanges()
        {
            throw new NotImplementedException(_messageExceptionSaveChanges);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new NotImplementedException(_messageExceptionSaveChanges);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
