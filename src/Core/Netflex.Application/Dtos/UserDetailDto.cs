namespace Netflex.Application.Dtos;

public record UserDetailDto(string Email, IEnumerable<string>? Roles, IEnumerable<string>? Permissions);