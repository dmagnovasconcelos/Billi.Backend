﻿using Billi.Backend.CrossCutting.UnitsOfWork;

namespace Billi.Backend.Domain.Interfaces.Users
{
    public interface IUserUnitOfWork : IUnitOfWork
    {
        IUserCommandRepository UserRepository { get; }  
        IUserRefreshTokenCommandRepository RefreshTokenRepository { get; }
    }
}