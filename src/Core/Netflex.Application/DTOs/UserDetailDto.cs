namespace Netflex.Application.DTOs;

public record UserDetailDto(string Email, bool Confirmed, IEnumerable<string>? Roles, IEnumerable<string>? Permissions);