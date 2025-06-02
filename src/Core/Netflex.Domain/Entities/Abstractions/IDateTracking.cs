namespace Netflex.Domain.Entities.Abstractions;

public interface IDateTracking
{
    DateTime? CreatedAt { get; set; }
    DateTime? LastModified { get; set; }
}
