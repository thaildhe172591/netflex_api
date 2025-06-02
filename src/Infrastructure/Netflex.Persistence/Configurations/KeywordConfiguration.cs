using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflex.Domain.Entities;

namespace Netflex.Persistence.Configurations;

public class KeywordConfiguration : IEntityTypeConfiguration<Keyword>
{
    public void Configure(EntityTypeBuilder<Keyword> builder)
    {
        builder.ToTable(nameof(Keyword).Pluralize().ToSnakeCase())
            .HasKey(k => k.Id);

        builder.Property(k => k.Name)
            .IsRequired();

        builder.HasIndex(k => k.Name).IsUnique();
    }
}