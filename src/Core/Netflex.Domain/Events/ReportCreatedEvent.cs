namespace Netflex.Domain.Events;

public record ReportCreatedEvent(Report Report) : IDomainEvent;