namespace Netflex.Application.DTOs;

public record MovieDetailDto
{
    public long Id { get; init; }
    public string? Title { get; init; }
    public string? Overview { get; init; }
    public string? PosterPath { get; init; }
    public string? BackdropPath { get; init; }
    public string? VideoUrl { get; init; }
    public string? CountryIso { get; init; }
    public int? Runtime { get; init; }
    public DateTime? ReleaseDate { get; init; }
    public decimal? AverageRating { get; init; }
    public int TotalReviews { get; init; }
    public ICollection<ActorDto>? Actors { get; init; }
    public ICollection<KeywordDto>? Keywords { get; init; }
    public ICollection<KeywordDto>? Genres { get; init; }
}
