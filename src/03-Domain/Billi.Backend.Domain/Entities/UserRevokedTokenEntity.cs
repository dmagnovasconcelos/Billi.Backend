using Billi.Backend.CrossCutting.Entities;

namespace Billi.Backend.Domain.Entities
{
    public class UserRevokedTokenEntity : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public UserEntity User { get; set; }
    }
}
