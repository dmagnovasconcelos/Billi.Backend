using Billi.Backend.CrossCutting.Repositories;
using Billi.Backend.Domain.Entities;

namespace Billi.Backend.Domain.Interfaces.Users
{
    public interface IUserCommandRepository : IGenericCommandRepository<UserEntity>
    { }
}