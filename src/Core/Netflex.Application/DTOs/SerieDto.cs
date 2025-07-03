namespace Netflex.Application.DTOs;

public record SerieDto
(
    long Id,
    string Name,
    string? Overview,
    string? PosterPath,
    string? BackdropPath,
    string? CountryIso,
    DateTime? FirstAirDate,
    DateTime? LastAirDate
);