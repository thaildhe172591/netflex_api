using Netflex.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netflex.Persistence.Configurations;

public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.ToTable(nameof(UserLogin).Pluralize().ToSnakeCase())
            .HasKey(x => new { x.LoginProvider, x.ProviderKey });

        builder.Property(x => x.LoginProvider)
            .HasConversion(n => n.Value, value => LoginProvider.Of(value))
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.ProviderKey)
            .HasMaxLength(256).IsRequired();
    }
}