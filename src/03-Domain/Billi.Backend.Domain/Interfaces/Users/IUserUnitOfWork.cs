using Billi.Backend.CrossCutting.UnitsOfWork;
using Billi.Backend.Domain.Entities;

namespace Billi.Backend.Domain.Interfaces.Users
{
    public interface IUserUnitOfWork : IUnitOfWork<UserEntity>
    {
        IUserRefreshTokenCommandRepository RefreshTokenRepository { get; }
        IUserRevokedTokenCommandRepository RevokedTokenRepository { get; }
    }
}
