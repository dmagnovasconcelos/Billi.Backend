using Billi.Backend.CrossCutting.EntityConfigurations;
using Billi.Backend.CrossCutting.Enums;
using Billi.Backend.CrossCutting.Utilities;
using Billi.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace Billi.Backend.Infra.Data.EntityConfigurations
{
    public class UserEntityConfiguration(IConfiguration configuration) : SoftDeleteBaseEntityConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Email).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Status).HasDefaultValue(StatusType.Active);

            builder.Property(x => x.Password)
                .IsRequired()
                .HasConversion(new EncryptedStringConverter(configuration))
                .HasMaxLength(512);
        }
    }
}
