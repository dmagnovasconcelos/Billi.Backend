using Billi.Backend.CrossCutting.Contexts;
using Billi.Backend.CrossCutting.Repositories;
using Billi.Backend.Domain.Entities;
using Billi.Backend.Domain.Interfaces.Users;

namespace Billi.Backend.Infra.Data.Repositories
{
    public class UserQueryRepository(BaseDbContext context) : GenericQueryRepository<UserEntity>(context), IUserQueryRepository
    { }
}