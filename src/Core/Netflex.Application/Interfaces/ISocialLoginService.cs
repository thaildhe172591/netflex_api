using Netflex.Domain.ValueObjects;

namespace Netflex.Application.Interfaces;

public record UserInfo(string Id, string Name,
   string Email, string Picture);

public interface ISocialLoginService
{
    LoginProvider Provider { get; }
    Task<string> GetLoginUrlAsync(string provider);
    Task<UserInfo> FetchUserInfoAsync(string code, string redirect);
}