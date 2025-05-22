using Billi.Backend.CrossCutting.Contexts;
using Billi.Backend.Infra.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Billi.Backend.Infra.Data.Contexts
{
    public class SqlDbContext(DbContextOptions<BaseDbContext> options, IConfiguration configuration, Guid? usuarioId = null) 
        : BaseDbContext(options, usuarioId)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "pt_BR.UTF-8");

            modelBuilder.ApplyConfiguration(new UserEntityConfiguration(configuration));
        }
    }
}
