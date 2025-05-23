using Billi.Backend.CrossCutting.Entities;

namespace Billi.Backend.CrossCutting.EntityConfigurations
{
    public abstract class AuditableBaseEntityConfiguration<T> : BaseEntityConfiguration<T>
        where T : AuditableBaseEntity
    { }
}
