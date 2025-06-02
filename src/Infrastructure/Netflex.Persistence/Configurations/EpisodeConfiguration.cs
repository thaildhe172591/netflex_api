using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netflex.Persistence.Configurations;

public class EpisodeConfiguration
    : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.ToTable(nameof(Episode).Pluralize().ToSnakeCase())
            .HasKey(x => x.Id);

        builder.Property(t => t.Id)
            .HasColumnName($"{nameof(Episode)}{nameof(Episode.Id)}".ToSnakeCase())
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(t => t.EpisodeNumber)
            .IsRequired();

        builder.Property(t => t.Overview)
            .HasMaxLength(1024);

        builder.Property(t => t.VideoURL)
            .HasMaxLength(256);

        builder.Property(t => t.Runtime);

        builder.Property(t => t.AirDate);

        builder.Property(t => t.SeriesId)
            .IsRequired();

        builder.HasMany(e => e.Actors)
            .WithMany()
            .UsingEntity($"{nameof(Episode)}{nameof(Actor)}".Pluralize().ToSnakeCase());
    }
}