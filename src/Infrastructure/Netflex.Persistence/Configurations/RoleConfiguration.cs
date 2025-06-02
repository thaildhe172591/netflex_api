using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netflex.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(nameof(Role).Pluralize().ToSnakeCase())
            .HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName($"{nameof(Role)}{nameof(Role.Id)}".ToSnakeCase());

        builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity($"{nameof(Role)}{nameof(Permission)}".Pluralize().ToSnakeCase());
    }
}