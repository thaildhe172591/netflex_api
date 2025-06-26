namespace Netflex.Domain.Entities;

public class Episode : Aggregate<long>
{
    public required string Name { get; set; }
    public int EpisodeNumber { get; set; }
    public string? Overview { get; set; }
    public string? VideoUrl { get; set; }
    public TimeSpan? Runtime { get; set; }
    public DateTime? AirDate { get; set; }
    public required long SeriesId { get; set; }
    public virtual ICollection<Actor> Actors { get; set; } = [];
    public static Episode Create(string name, int episodeNumber, long seriesId, string? overview = default,
        string? videoUrl = default, TimeSpan? runtime = default, DateTime? airDate = default)
    {
        var episode = new Episode()
        {
            Name = name,
            EpisodeNumber = episodeNumber,
            SeriesId = seriesId,
            Overview = overview,
            VideoUrl = videoUrl,
            Runtime = runtime,
            AirDate = airDate
        };
        episode.AddDomainEvent(new EpisodeCreatedEvent(episode));
        return episode;
    }
}