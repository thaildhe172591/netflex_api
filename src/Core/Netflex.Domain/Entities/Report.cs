
using Netflex.Domain.Enumerations;

namespace Netflex.Domain.Entities;

public class Report : AuditableEntity<long>
{
    public required string Reason { get; set; }
    public string? Description { get; set; }
    public Process Process { get; set; } = Process.Open;
    public static Report Create(string reason, string? description, Process process)
    {
        var report = new Report
        {
            Reason = reason,
            Description = description,
            Process = process
        };
        report.AddDomainEvent(new ReportCreatedEvent(report));
        return report;
    }

    public Report Update(string? reason, string? description)
    {
        Reason = reason ?? Reason;
        Description = description ?? Description;
        return this;
    }
}
