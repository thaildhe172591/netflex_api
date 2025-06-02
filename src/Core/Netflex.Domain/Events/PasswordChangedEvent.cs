namespace Netflex.Domain.Events;

public record PasswordChangedEvent(User User, string? SessionId) : IDomainEvent;