using Netflex.Domain.Events;

namespace Netflex.Domain.Entities.Abstractions;

public interface IAggregate<T> : IAggregate, IEntity<T>;

public interface IAggregate : IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IDomainEvent[] ClearDomainEvents();
}