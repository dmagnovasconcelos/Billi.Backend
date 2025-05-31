using Billi.Backend.CrossCutting.Entities;
using Billi.Backend.CrossCutting.Enums;

namespace Billi.Backend.Domain.Entities
{
    public class UserEntity : SoftDeleteBaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public StatusType Status { get; set; }
        public bool RedefinePassword { get; set; }
        public string ValidationResetPassword { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? SupplierId { get; set; }
        public UserRefreshTokenEntity UserRefreshToken { get; set; }
        public bool IsSystemUser { get; private set; } = false;
    }
}