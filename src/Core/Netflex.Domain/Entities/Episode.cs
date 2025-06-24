namespace Netflex.Domain.Entities;

public class Episode : Aggregate<long>
{
    public required string Name { get; set; }
    public int EpisodeNumber { get; set; }
    public string? Overview { get; set; }
    public string? VideoURL { get; set; }
    public TimeSpan? Runtime { get; set; }
    public DateTime? AirDate { get; set; }
    public required long SeriesId { get; set; }
    public virtual ICollection<Actor> Actors { get; set; } = [];
    public static Episode Create(string name, int episodeNumber, long seriesId, string? overview = default,
        string? videoURL = default, TimeSpan? runtime = default, DateTime? airDate = default)
    {
        var episode = new Episode()
        {
            Name = name,
            EpisodeNumber = episodeNumber,
            SeriesId = seriesId,
            Overview = overview,
            VideoURL = videoURL,
            Runtime = runtime,
            AirDate = airDate
        };
        episode.AddDomainEvent(new EpisodeCreatedEvent(episode));
        return episode;
    }
}