using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflex.Domain.ValueObjects;

namespace Netflex.Persistence.Configurations;

public class MovieConfiguration
    : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable(nameof(Movie).Pluralize().ToSnakeCase())
            .HasKey(x => x.Id);

        builder.Property(m => m.Id)
            .HasColumnName($"{nameof(Movie)}{nameof(Movie.Id)}".ToSnakeCase())
            .ValueGeneratedOnAdd();

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(m => m.Overview)
            .HasMaxLength(1024);

        builder.Property(m => m.PosterPath)
            .HasMaxLength(256);

        builder.Property(m => m.BackdropPath)
            .HasMaxLength(256);

        builder.Property(m => m.VideoURL)
            .HasMaxLength(256);

        builder.Property(m => m.CountryISO)
            .HasMaxLength(3);

        builder.Property(m => m.RunTime);

        builder.Property(m => m.ReleaseDate);

        builder.HasMany(m => m.Genres)
            .WithMany()
            .UsingEntity($"{nameof(Movie)}{nameof(Genre)}".Pluralize().ToSnakeCase());

        builder.HasMany(m => m.Keywords)
            .WithMany()
            .UsingEntity($"{nameof(Movie)}{nameof(Keyword)}".Pluralize().ToSnakeCase());

        builder.HasMany(m => m.Actors)
            .WithMany()
            .UsingEntity($"{nameof(Movie)}{nameof(Actor)}".Pluralize().ToSnakeCase());

    }
}