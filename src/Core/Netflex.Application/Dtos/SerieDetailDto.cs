namespace Netflex.Application.DTOs;

public record SerieDetailDto
{
    public long Id { get; init; }
    public string? Name { get; init; }
    public string? Overview { get; init; }
    public string? PosterPath { get; init; }
    public string? BackdropPath { get; init; }
    public string? CountryIso { get; init; }
    public DateOnly? FirstAirDate { get; init; }
    public DateOnly? LastAirDate { get; init; }
    public ICollection<KeywordDto>? Keywords { get; init; }
    public ICollection<GenreDto>? Genres { get; init; }
}