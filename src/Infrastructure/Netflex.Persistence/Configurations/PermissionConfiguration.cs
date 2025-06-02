using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netflex.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(nameof(Permission).Pluralize().ToSnakeCase()).HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName($"{nameof(Permission)}{nameof(Permission.Id)}".ToSnakeCase());

        builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique();
    }
}