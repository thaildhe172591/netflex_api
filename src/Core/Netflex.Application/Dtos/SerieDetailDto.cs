namespace Netflex.Application.Dtos;

public record SerieDetailDto
(
    long Id,
    string Name,
    string? Overview,
    string? PosterPath,
    string? BackdropPath,
    string? CountryISO,
    DateTime? FirstAirDate,
    DateTime? LastAirDate,
    ICollection<KeywordDto>? Keywords,
    ICollection<GenreDto>? Genres
);