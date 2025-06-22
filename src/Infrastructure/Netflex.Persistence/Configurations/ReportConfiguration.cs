using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflex.Domain.Enumerations;

namespace Netflex.Persistence.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.ToTable(nameof(Report).Pluralize().ToSnakeCase())
            .HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName($"{nameof(Report)}{nameof(Report.Id)}".ToSnakeCase())
            .ValueGeneratedOnAdd();

        builder.Property(r => r.Reason)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(r => r.Description)
            .HasMaxLength(1024);

        builder.Property(r => r.Process)
            .HasConversion<string>()
            .HasDefaultValue(Process.Open)
            .IsRequired();
    }
}