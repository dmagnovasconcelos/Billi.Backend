using Billi.Backend.CrossCutting.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Billi.Backend.CrossCutting.Contexts
{
    public class BaseDbContext(DbContextOptions<BaseDbContext> options, Guid? usuarioId = null) : DbContext(options)
    {

        private const string _messageExceptionSaveChanges = "Use SaveChangesAsync instead of SaveChanges.";
        
        public override EntityEntry Remove(object entity)
        {
            if (entity is SoftDeleteBaseEntity softDeleteBaseEntity)
            {
                EntityEntry<SoftDeleteBaseEntity> entityEntry = Entry(softDeleteBaseEntity);
                entityEntry.Entity.IsDeleted = true;
                entityEntry.Entity.DeletedAt = DateTime.Now;
                entityEntry.Entity.DeletedBy = usuarioId ?? Guid.Empty;
                entityEntry.State = EntityState.Modified;

                return entityEntry;
            }
            else
            {
                return base.Remove(entity);
            }
        }

        public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        {
            if (entity is SoftDeleteBaseEntity softDeleteBaseEntity)
            {
                EntityEntry<SoftDeleteBaseEntity> entityEntry = Entry(softDeleteBaseEntity);
                entityEntry.Entity.IsDeleted = true;
                entityEntry.Entity.DeletedAt = DateTime.Now;
                entityEntry.Entity.DeletedBy = usuarioId ?? Guid.Empty;
                entityEntry.State = EntityState.Modified;
                
                return entityEntry as EntityEntry<TEntity>;
            }
            else
            {
                return base.Remove(entity);
            }
        }

        public override void RemoveRange(params object[] entities)
        {
            foreach (var entity in entities)
                Remove(entity);
        }

        public override void RemoveRange(IEnumerable<object> entities)
        {
            foreach (var entity in entities)
                Remove(entity);
        }

        public override int SaveChanges()
        {
            throw new NotImplementedException(_messageExceptionSaveChanges);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new NotImplementedException(_messageExceptionSaveChanges);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await base.Database.BeginTransactionAsync(cancellationToken);
                foreach (EntityEntry item in base.ChangeTracker.Entries())
                {
                    if (item.Entity is AuditableBaseEntity entity)
                    {
                        switch (item.State)
                        {
                            case EntityState.Added:
                                entity.CreatedAt = DateTime.Now;
                                entity.CreatedBy = usuarioId ?? Guid.Empty;
                                break;
                            case EntityState.Modified:
                                entity.UpdatedAt = DateTime.Now;
                                entity.UpdatedBy = usuarioId ?? Guid.Empty;
                                break;
                        }
                    }
                }

                int ret = await base.SaveChangesAsync(cancellationToken);
                await base.Database.CommitTransactionAsync(cancellationToken);
                return ret;
            }
            catch
            {
                await base.Database.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
