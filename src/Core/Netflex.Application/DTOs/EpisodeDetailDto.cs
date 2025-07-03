namespace Netflex.Application.DTOs;

public record EpisodeDetailDto
{
    public long Id { get; init; }
    public string? Name { get; init; }
    public int EpisodeNumber { get; init; }
    public string? Overview { get; init; }
    public string? VideoUrl { get; init; }
    public TimeSpan? Runtime { get; init; }
    public DateTime? AirDate { get; init; }
    public long SeriesId { get; init; }
    public string? SeriesName { get; init; }
    public string? PosterPath { get; init; }
    public ICollection<ActorDto>? Actors { get; init; }
}