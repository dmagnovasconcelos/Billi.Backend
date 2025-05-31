using Billi.Backend.CrossCutting.Entities;

namespace Billi.Backend.Domain.Entities
{
    public class UserRefreshTokenEntity : AuditableBaseEntity
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public UserEntity User { get; set; }
    }
}