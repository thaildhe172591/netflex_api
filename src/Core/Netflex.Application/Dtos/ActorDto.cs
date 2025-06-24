namespace Netflex.Application.Dtos;

public class ActorDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public bool? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Biography { get; set; }
}