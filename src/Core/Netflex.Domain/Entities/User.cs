namespace Netflex.Domain.Entities;

public class User : Aggregate<string>
{
    public required Email Email { get; set; }
    public HashString? PasswordHash { get; private set; }
    public bool EmailConfirmed { get; set; }
    public int Version { get; set; }
    public virtual ICollection<Role> Roles { get; set; } = [];
    public virtual ICollection<Permission> Permissions { get; set; } = [];
    public virtual ICollection<UserSession> UserSessions { get; set; } = [];
    public virtual ICollection<UserLogin> UserLogins { get; set; } = [];
    public static User Create(string id, Email email, HashString passwordHash, bool emailConfirmed = false)
    {
        var user = Create(id, email, emailConfirmed);
        user.PasswordHash = passwordHash;
        return user;
    }

    public static User Create(string id, Email email, bool emailConfirmed = false)
    {
        var user = new User() { Id = id, Email = email, EmailConfirmed = emailConfirmed };
        user.AddDomainEvent(new UserCreatedEvent(user));
        return user;
    }

    public void ChangePassword(string password, string? sessionId = default)
    {
        PasswordHash = HashString.Of(password);
        AddDomainEvent(new PasswordChangedEvent(this, sessionId));
    }

    public void AssignRole(List<Role> roles)
    {
        if (roles == null || roles.Count == 0) return;
        Roles.Clear();
        foreach (var role in roles)
        {
            if (role == null) continue;
            Roles.Add(role);
        }
    }

}