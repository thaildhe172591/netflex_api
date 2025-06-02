namespace Netflex.Domain.Events;

public record UserCreatedEvent(User User) : IDomainEvent;