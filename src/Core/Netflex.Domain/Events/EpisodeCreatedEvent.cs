namespace Netflex.Domain.Events;

public record EpisodeCreatedEvent(Episode Episode) : IDomainEvent;