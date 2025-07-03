namespace Netflex.Application.DTOs;

public record ActorDto
{
    public long Id { get; init; }
    public string? Name { get; init; }
    public string? Image { get; init; }
    public bool? Gender { get; init; }
    public DateTime? BirthDate { get; init; }
    public string? Biography { get; init; }
}