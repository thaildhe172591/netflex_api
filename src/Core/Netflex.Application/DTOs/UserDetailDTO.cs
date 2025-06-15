namespace Netflex.Application.DTOs;

public record UserDetailDTO(string Email, IEnumerable<string>? Roles, IEnumerable<string>? Permissions);