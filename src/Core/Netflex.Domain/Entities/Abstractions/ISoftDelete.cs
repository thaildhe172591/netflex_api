namespace Netflex.Domain.Entities.Abstractions;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
}