namespace Netflex.Application.Dtos;

public record MovieDetailDto(
    long Id,
    string Title,
    string? Overview,
    string? PosterPath,
    string? BackdropPath,
    string? VideoUrl,
    string? CountryIso,
    TimeSpan? RunTime,
    DateTime? ReleaseDate,
    ICollection<ActorDto>? Actors,
    ICollection<KeywordDto>? Keywords,
    ICollection<KeywordDto>? Genres
);
