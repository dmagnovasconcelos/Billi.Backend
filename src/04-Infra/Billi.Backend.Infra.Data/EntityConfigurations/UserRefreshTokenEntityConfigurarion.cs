using Billi.Backend.CrossCutting.EntityConfigurations;
using Billi.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billi.Backend.Infra.Data.EntityConfigurations
{
    public class UserRefreshTokenEntityConfigurarion : AuditableBaseEntityConfiguration<UserRefreshTokenEntity>
    {
        public override void Configure(EntityTypeBuilder<UserRefreshTokenEntity> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Token).HasMaxLength(100).IsRequired();

            builder.HasOne(x => x.User)
                .WithOne(x => x.UserRefreshToken)
                .HasForeignKey<UserRefreshTokenEntity>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}