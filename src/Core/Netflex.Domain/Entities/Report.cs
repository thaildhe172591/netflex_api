
using Netflex.Domain.Enumerations;

namespace Netflex.Domain.Entities;

public class Report : AuditableEntity<long>
{
    public required string Reason { get; set; }
    public string? Description { get; set; }
    public Process Process { get; set; } = Process.Open;
    public static Report Create(string reason, string description, Process process) => new()
    {
        Reason = reason,
        Description = description,
        Process = process
    };
}
