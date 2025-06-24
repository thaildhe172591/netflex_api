namespace Netflex.Application.Dtos;

public record MovieDto(
    long Id,
    string Title,
    string? Overview,
    string? PosterPath,
    string? BackdropPath,
    string? VideoURL,
    string? CountryISO,
    TimeSpan? RunTime,
    DateTime? ReleaseDate,
    ICollection<ActorDto>? Actors,
    ICollection<KeywordDto>? Keywords,
    ICollection<KeywordDto>? Genres
);
