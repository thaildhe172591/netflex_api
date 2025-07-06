namespace Netflex.Application.DTOs;

public record EpisodeDto
{
    public long Id { get; init; }
    public string? Name { get; init; }
    public int EpisodeNumber { get; init; }
    public string? Overview { get; init; }
    public string? VideoUrl { get; init; }
    public int? Runtime { get; init; }
    public DateOnly? AirDate { get; init; }
    public long SeriesId { get; init; }
    public string? SeriesName { get; init; }
    public string? PosterPath { get; init; }
}