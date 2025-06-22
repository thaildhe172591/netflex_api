using Netflex.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netflex.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User).Pluralize().ToSnakeCase())
            .HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnName($"{nameof(User)}{nameof(User.Id)}".ToSnakeCase());

        builder.Property(u => u.Email)
            .HasConversion(n => n.Value, value => Email.Of(value))
            .HasMaxLength(256);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.EmailConfirmed);
        builder.Property(u => u.Version);

        builder.Property(u => u.PasswordHash)
            .HasConversion(
                n => n != null ? n.Value : null,
                value => value != null ? new HashString(value) : null
            );

        builder.HasMany(u => u.UserLogins)
            .WithOne()
            .HasForeignKey(x => x.UserId);

        builder.HasMany(u => u.UserSessions)
            .WithOne()
            .HasForeignKey(x => x.UserId);

        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<Dictionary<string, object>>(
                $"{nameof(User)}{nameof(Role)}".Pluralize().ToSnakeCase(),
                right => right.HasOne<Role>().WithMany().HasForeignKey($"{nameof(Role)}{nameof(Role.Id)}".ToSnakeCase()),
                left => left.HasOne<User>().WithMany().HasForeignKey($"{nameof(User)}{nameof(User.Id)}".ToSnakeCase()));

        builder.HasMany(u => u.Permissions)
            .WithMany(p => p.Users)
            .UsingEntity<Dictionary<string, object>>(
                $"{nameof(User)}{nameof(Permission)}".Pluralize().ToSnakeCase(),
                right => right.HasOne<Permission>().WithMany().HasForeignKey($"{nameof(Permission)}{nameof(Permission.Id)}".ToSnakeCase()),
                left => left.HasOne<User>().WithMany().HasForeignKey($"{nameof(User)}{nameof(User.Id)}".ToSnakeCase()));
    }
}