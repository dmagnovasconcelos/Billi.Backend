using Billi.Backend.CrossCutting.Auth;
using Billi.Backend.CrossCutting.Contexts;
using Billi.Backend.CrossCutting.Utilities;
using Billi.Backend.Infra.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Billi.Backend.Infra.Data.Contexts
{
    public class SqlDbContext(DbContextOptions<SqlDbContext> options, IConfiguration configuration, ICurrentUser currentUser)
        : BaseDbContext(options, currentUser.UserId)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration(configuration));
            modelBuilder.ApplyConfiguration(new UserRefreshTokenEntityConfigurarion());

            base.OnModelCreating(modelBuilder);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(Utility.ToSnakeCase(entity.GetTableName()?.Replace("entity", string.Empty, StringComparison.InvariantCultureIgnoreCase)));

                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(Utility.ToSnakeCase(property.Name));

                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetColumnType("timestamp");
                    }
                }

                foreach (var key in entity.GetKeys())
                    key.SetName(Utility.ToSnakeCaseKeepPrefix(key.GetName(), "PK"));

                foreach (var fk in entity.GetForeignKeys())
                    fk.SetConstraintName(Utility.ToSnakeCaseKeepPrefix(fk.GetConstraintName(), "FK"));

                foreach (var index in entity.GetIndexes())
                    index.SetDatabaseName(Utility.ToSnakeCaseKeepPrefix(index.GetDatabaseName(), "IX"));
            }
        }
    }
}