﻿using Billi.Backend.CrossCutting.Repositories;
using Billi.Backend.Domain.Entities;
using Billi.Backend.Domain.Interfaces.Users;
using Billi.Backend.Infra.Data.Contexts;

namespace Billi.Backend.Infra.Data.Repositories
{
    public class UserCommandRepository(SqlDbContext context) : GenericCommandRepository<UserEntity>(context), IUserCommandRepository
    { }
}