namespace Netflex.Domain.Entities.Abstractions;

public abstract class AuditableEntity<T> : Aggregate<T>, IAuditable
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastModified { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastModifiedBy { get; set; }
}