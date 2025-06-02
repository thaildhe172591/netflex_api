namespace Netflex.Domain.Events;

public record TVSerieCreatedEvent(TVSerie TVSerie) : IDomainEvent;