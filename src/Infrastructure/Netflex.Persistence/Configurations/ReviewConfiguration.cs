using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflex.Domain.ValueObjects;

namespace Netflex.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable(nameof(Review).Pluralize().ToSnakeCase())
            .HasKey(r => new { r.UserId, r.TargetId, r.TargetType });

        builder.Property(f => f.TargetId)
            .IsRequired();

        builder.Property(r => r.TargetType)
            .HasConversion(tt => tt.Value, value => TargetType.Of(value))
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.UserId)
            .IsRequired();

        builder.Property(r => r.Rating)
            .HasConversion(r => r.Value, value => Rating.Of(value))
            .IsRequired();

        builder.Property(r => r.Comment)
            .HasMaxLength(500);

        builder.Property(r => r.LikeCount);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(r => r.UserId);
    }
}