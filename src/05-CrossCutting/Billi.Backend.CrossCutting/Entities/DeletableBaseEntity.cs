namespace Billi.Backend.CrossCutting.Entities
{
    public class DeletableBaseEntity : AuditableBaseEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid DeletedBy { get; set; }

        public DeletableBaseEntity()
        {
            IsDeleted = false;
        }

        public void SetDeleted(Guid userId)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = userId;
        }
    }
}
