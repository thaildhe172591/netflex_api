using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netflex.Persistence.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable(nameof(Genre).Pluralize().ToSnakeCase())
            .HasKey(g => g.Id);

        builder.Property(g => g.Name)
            .IsRequired();

        builder.HasIndex(g => g.Name).IsUnique();
    }
}