using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Netflex.Persistence.Configurations;

public class TVSerieConfiguration
    : IEntityTypeConfiguration<TVSerie>
{
    public void Configure(EntityTypeBuilder<TVSerie> builder)
    {
        builder.ToTable(nameof(TVSerie).Pluralize().ToSnakeCase())
            .HasKey(x => x.Id);

        builder.Property(t => t.Id)
            .HasColumnName($"{nameof(TVSerie)}{nameof(TVSerie.Id)}".ToSnakeCase())
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(t => t.Overview)
            .HasMaxLength(1024);

        builder.Property(t => t.PosterPath)
            .HasMaxLength(256);

        builder.Property(t => t.BackdropPath)
            .HasMaxLength(256);

        builder.Property(t => t.CountryISO)
            .HasMaxLength(3);

        builder.Property(t => t.FirstAirDate);

        builder.Property(t => t.LastAirDate);

        builder.HasMany<Episode>()
            .WithOne()
            .HasForeignKey(e => e.SeriesId);

        builder.HasMany(t => t.Genres)
            .WithMany()
            .UsingEntity($"{nameof(TVSerie)}{nameof(Genre)}".Pluralize().ToSnakeCase());

        builder.HasMany(t => t.Keywords)
            .WithMany()
            .UsingEntity($"{nameof(TVSerie)}{nameof(Keyword)}".Pluralize().ToSnakeCase());
    }
}