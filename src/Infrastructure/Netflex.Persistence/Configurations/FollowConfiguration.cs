using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflex.Domain.ValueObjects;

namespace Netflex.Persistence.Configurations;

public class FollowConfiguration : IEntityTypeConfiguration<Follow>
{
    public void Configure(EntityTypeBuilder<Follow> builder)
    {
        builder.ToTable(nameof(Follow).Pluralize().ToSnakeCase())
            .HasKey(f => new { f.UserId, f.TargetId, f.TargetType });

        builder.Property(f => f.TargetId)
            .IsRequired();

        builder.Property(f => f.TargetType)
            .HasConversion(tt => tt.Value, value => TargetType.Of(value))
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(f => f.UserId)
            .IsRequired();

        builder.Property(f => f.CreatedAt)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(f => f.UserId);
    }
}