namespace Netflex.Application.DTOs;

public record MovieDto(
    long Id,
    string Title,
    string? Overview,
    string? PosterPath,
    string? BackdropPath,
    string? VideoUrl,
    string? CountryIso,
    TimeSpan? RunTime,
    DateTime? ReleaseDate
);
