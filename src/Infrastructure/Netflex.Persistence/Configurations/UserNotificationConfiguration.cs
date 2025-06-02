using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netflex.Persistence.Configurations;

public class UserNotificationConfiguration
    : IEntityTypeConfiguration<UserNotification>
{
    public void Configure(EntityTypeBuilder<UserNotification> builder)
    {
        builder.ToTable(nameof(UserNotification).Pluralize().ToSnakeCase())
            .HasKey(x => new { x.UserId, x.NotificationId });

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(un => un.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Notification>()
            .WithMany()
            .HasForeignKey(un => un.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(un => un.HaveRead).IsRequired();
    }
}