namespace Netflex.Domain.Entities;

public class UserLogin : IEntity
{
    public required string UserId { get; set; }
    public required LoginProvider LoginProvider { get; set; }
    public required string ProviderKey { get; set; }
    private UserLogin() { }
    public static UserLogin Create(string userId, LoginProvider loginProvider, string providerKey)
    => new() { UserId = userId, LoginProvider = loginProvider, ProviderKey = providerKey };
}