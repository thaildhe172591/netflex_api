namespace Netflex.Application.DTOs;

public record UserDTO(string Email, IEnumerable<string>? Roles, IEnumerable<string>? Permissions);