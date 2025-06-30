namespace Netflex.Application.Dtos;

public record UserDetailDto(string Email, bool Confirmed, IEnumerable<string>? Roles, IEnumerable<string>? Permissions);