using MediatR;

namespace Netflex.Domain.Events;

public interface IDomainEvent : INotification
{
    Guid EventId => Guid.NewGuid();
    DateTime OccurredOn => DateTime.Now;
    string? EventType => GetType().AssemblyQualifiedName;
}
