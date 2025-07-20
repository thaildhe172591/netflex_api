namespace Netflex.Application.DTOs;

public class UserDto
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public bool Confirmed { get; init; }
    public string Roles { get; init; } = string.Empty;
    public string Permissions { get; init; } = string.Empty;
}