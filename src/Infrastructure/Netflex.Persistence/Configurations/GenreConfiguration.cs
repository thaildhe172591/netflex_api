using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netflex.Persistence.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable(nameof(Genre).Pluralize().ToSnakeCase())
            .HasKey(g => g.Id);

        builder.Property(t => t.Id)
            .HasColumnName($"{nameof(Genre)}{nameof(Genre.Id)}".ToSnakeCase())
            .ValueGeneratedOnAdd();

        builder.Property(g => g.Name)
            .IsRequired();

        builder.HasIndex(g => g.Name).IsUnique();
    }
}