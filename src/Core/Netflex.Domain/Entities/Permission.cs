namespace Netflex.Domain.Entities;

public class Permission : Entity<string>
{
    public required string Name { get; set; }
    public virtual ICollection<User> Users { get; set; } = [];
    public virtual ICollection<Role> Roles { get; set; } = [];
    private Permission() { }
    public static Permission Create(string id, string name)
        => new() { Id = id, Name = name };
}