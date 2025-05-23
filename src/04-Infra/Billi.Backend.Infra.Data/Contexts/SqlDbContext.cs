using Billi.Backend.CrossCutting.Contexts;
using Billi.Backend.Domain.ValueObjects;
using Billi.Backend.Infra.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Billi.Backend.Infra.Data.Contexts
{
    public class SqlDbContext(DbContextOptions<BaseDbContext> options, IConfiguration configuration, AccessToken accessToken)
        : BaseDbContext(options, accessToken.UserId)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "pt_BR.UTF-8");

            modelBuilder.ApplyConfiguration(new UserEntityConfiguration(configuration));
            modelBuilder.ApplyConfiguration(new UserRefreshTokenEntityConfigurarion());
            modelBuilder.ApplyConfiguration(new UserRevokedTokenEntityConfiguration());
        }
    }
}
