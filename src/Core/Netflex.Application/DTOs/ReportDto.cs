namespace Netflex.Application.DTOs;

public record ReportDto
{
    public long Id { get; init; }
    public string? Reason { get; init; }
    public string? Description { get; init; }
    public string? Process { get; init; }
}