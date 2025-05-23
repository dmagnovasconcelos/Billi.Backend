namespace Billi.Backend.CrossCutting.Entities
{
    public abstract class AuditableBaseEntity : BaseEntity
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
