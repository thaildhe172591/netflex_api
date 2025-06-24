namespace Netflex.Domain.Entities;

public class Role : Entity<string>
{
    public required string Name { get; set; }
    public virtual ICollection<User> Users { get; set; } = [];
    public virtual ICollection<Permission> Permissions { get; set; } = [];
    public static Role Create(string id, string name)
        => new() { Id = id, Name = name };
}