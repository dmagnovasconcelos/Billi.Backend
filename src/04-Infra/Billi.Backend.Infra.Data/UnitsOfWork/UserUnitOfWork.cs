using Billi.Backend.CrossCutting.UnitsOfWork;
using Billi.Backend.Domain.Entities;
using Billi.Backend.Domain.Interfaces.Users;
using Billi.Backend.Infra.Data.Contexts;

namespace Billi.Backend.Infra.Data.UnitsOfWork
{
    public class UserUnitOfWork(SqlDbContext context, IUserRefreshTokenCommandRepository refreshTokenRepository, IUserRevokedTokenCommandRepository revokedTokenRepository) : UnitOfWork<UserEntity>(context), IUserUnitOfWork
    {
        public IUserRefreshTokenCommandRepository RefreshTokenRepository { get; } = refreshTokenRepository;

        public IUserRevokedTokenCommandRepository RevokedTokenRepository { get; } = revokedTokenRepository;
    }
}
