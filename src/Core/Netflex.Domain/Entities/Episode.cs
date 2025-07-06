namespace Netflex.Domain.Entities;

public class Episode : Aggregate<long>
{
    public required string Name { get; set; }
    public int EpisodeNumber { get; set; }
    public string? Overview { get; set; }
    public string? VideoUrl { get; set; }
    public int? Runtime { get; set; }
    public DateOnly? AirDate { get; set; }
    public required long SeriesId { get; set; }
    public virtual ICollection<Actor> Actors { get; set; } = [];
    public static Episode Create(string name, int episodeNumber, long seriesId, string? overview = default,
        string? videoUrl = default, int? runtime = default, DateOnly? airDate = default)
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

    public void AssignActors(IEnumerable<Actor> actors)
    {
        if (actors == null) return;
        Actors.Clear();
        foreach (var actor in actors)
        {
            Actors.Add(actor);
        }
    }

    public void Update(string? name, int? episodeNumber, long? seriesId, string? overview,
        string? videoUrl, int? runtime, DateOnly? airDate)
    {
        Name = name ?? Name;
        EpisodeNumber = episodeNumber ?? EpisodeNumber;
        SeriesId = seriesId ?? SeriesId;
        Overview = overview ?? Overview;
        VideoUrl = videoUrl ?? VideoUrl;
        Runtime = runtime ?? Runtime;
        AirDate = airDate ?? AirDate;
    }
}