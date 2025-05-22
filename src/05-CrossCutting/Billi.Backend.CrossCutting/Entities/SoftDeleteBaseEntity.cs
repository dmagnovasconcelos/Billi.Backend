namespace Billi.Backend.CrossCutting.Entities
{
    public class SoftDeleteBaseEntity : AuditableBaseEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid DeletedBy { get; set; }

        public SoftDeleteBaseEntity()
        {
            IsDeleted = false;
        }
    }
}
