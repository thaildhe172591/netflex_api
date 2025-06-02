namespace Netflex.Domain.Events;

public record MovieCreatedEvent(Movie Movie) : IDomainEvent;