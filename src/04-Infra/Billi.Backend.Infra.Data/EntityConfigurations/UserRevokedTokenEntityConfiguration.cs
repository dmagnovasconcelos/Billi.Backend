using Billi.Backend.CrossCutting.EntityConfigurations;
using Billi.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billi.Backend.Infra.Data.EntityConfigurations
{
    public class UserRevokedTokenEntityConfiguration : BaseEntityConfiguration<UserRevokedTokenEntity>
    {
        public override void Configure(EntityTypeBuilder<UserRevokedTokenEntity> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Token).IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserRevokedTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
