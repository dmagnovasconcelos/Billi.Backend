namespace Billi.Backend.CrossCutting.Entities
{
    public abstract class AuditableBaseEntity : BaseEntity
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public void SetCreated(Guid userId)
        {
            CreatedBy = userId;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetUpdated(Guid userId)
        {
            UpdatedBy = userId;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
