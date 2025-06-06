﻿using Billi.Backend.CrossCutting.Repositories;
using Billi.Backend.Domain.Entities;

namespace Billi.Backend.Domain.Interfaces.Users
{
    public interface IUserQueryRepository : IGenericQueryRepository<UserEntity>
    { }
}