namespace Netflex.Application.DTOs;

public class MovieDto
{
    public long Id { get; init; }
    public string? Title { get; init; }
    public string? Overview { get; init; }
    public string? PosterPath { get; init; }
    public string? BackdropPath { get; init; }
    public string? VideoUrl { get; init; }
    public string? CountryIso { get; init; }
    public int? Runtime { get; init; }
    public DateOnly? ReleaseDate { get; init; }
}
