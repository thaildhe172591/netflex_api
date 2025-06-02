using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflex.Domain.ValueObjects;

namespace Netflex.Persistence.Configurations;

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable(nameof(UserSession).Pluralize().ToSnakeCase())
            .HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName($"{nameof(UserSession)}{nameof(UserSession.Id)}".ToSnakeCase());

        builder.Property(x => x.RefreshHash)
            .HasConversion(n => n.Value, value => new HashString(value));

        builder.Property(x => x.DeviceId).HasMaxLength(256).IsRequired();
        builder.Property(x => x.DeviceInfo).HasMaxLength(256);
        builder.Property(x => x.IsRevoked);

        builder.HasIndex(u => new { u.UserId, u.DeviceId })
            .IsUnique()
            .HasFilter($"{nameof(UserSession.IsRevoked).ToSnakeCase()} = false");

        builder.Property(x => x.IpAddress).HasMaxLength(256);
        builder.Property(x => x.ExpiresAt).IsRequired();
    }
}