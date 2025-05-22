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
        public Guid? ValidationToken { get; set; }      
        
        public Guid? PersonId { get; set; }
        public Guid? SupplierId { get; set; }
        public bool IsSystemUser { get; private set; } = false;
    }
}
