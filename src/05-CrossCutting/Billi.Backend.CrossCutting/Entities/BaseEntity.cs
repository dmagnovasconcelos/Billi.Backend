namespace Billi.Backend.CrossCutting.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
