namespace Netflex.Application.DTOs;

public record SerieDto
{
    public long Id { get; init; }
    public string? Name { get; init; }
    public string? Overview { get; init; }
    public string? PosterPath { get; init; }
    public string? BackdropPath { get; init; }
    public string? CountryIso { get; init; }
    public DateTime? FirstAirDate { get; init; }
    public DateTime? LastAirDate { get; init; }
    public decimal? AverageRating { get; init; }
    public int TotalReviews { get; init; }
}
