using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netflex.Persistence.Configurations;

public class ActorConfiguration
    : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.ToTable(nameof(Actor).Pluralize().ToSnakeCase())
            .HasKey(x => x.Id);

        builder.Property(t => t.Id)
            .HasColumnName($"{nameof(Actor)}{nameof(Actor.Id)}".ToSnakeCase())
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(t => t.Image)
            .HasMaxLength(256);

        builder.Property(t => t.Gender);

        builder.Property(t => t.BirthDate);

        builder.Property(t => t.Biography)
            .HasMaxLength(1024);
    }
}