namespace Netflex.Application.Interfaces;

public interface IRefreshTokenService
{
    int ExpiresInDays { get; }
}
