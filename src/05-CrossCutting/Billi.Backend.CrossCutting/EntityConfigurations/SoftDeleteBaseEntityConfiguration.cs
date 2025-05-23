using Billi.Backend.CrossCutting.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billi.Backend.CrossCutting.EntityConfigurations
{
    public abstract class SoftDeleteBaseEntityConfiguration<T> : AuditableBaseEntityConfiguration<T>
        where T : SoftDeleteBaseEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName($"IX_{typeof(T).Name}_IsDeleted");

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
