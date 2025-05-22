using Billi.Backend.CrossCutting.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billi.Backend.CrossCutting.EntityConfigurations
{
    public abstract class AuditableBaseEntityConfiguration<T> : BaseEntityConfiguration<T> 
        where T : AuditableBaseEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired();
        }
    }
}
