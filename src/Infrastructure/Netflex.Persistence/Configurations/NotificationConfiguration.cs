using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netflex.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable(nameof(Notification).Pluralize().ToSnakeCase())
            .HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .HasColumnName($"{nameof(Notification)}{nameof(Notification.Id)}".ToSnakeCase())
            .ValueGeneratedOnAdd();

        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(n => n.Content)
            .HasMaxLength(1024);

        builder.Property(n => n.CreatedAt)
            .IsRequired();
    }
}