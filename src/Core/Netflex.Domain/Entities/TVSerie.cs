namespace Netflex.Domain.Entities;

public class TVSerie : Aggregate<long>
{
    public required string Name { get; set; }
    public string? Overview { get; set; }
    public string? PosterPath { get; set; }
    public string? BackdropPath { get; set; }
    public string? CountryIso { get; set; }
    public DateTime? FirstAirDate { get; set; }
    public DateTime? LastAirDate { get; set; }
    public virtual ICollection<Keyword> Keywords { get; set; } = [];
    public virtual ICollection<Genre> Genres { get; set; } = [];
    public static TVSerie Create(string name, string? overview = default, string? posterPath = default,
        string? backdropPath = default, DateTime? firstAirDate = default,
        DateTime? lastAirDate = default, string? countryIso = default)
    {
        var tvSerie = new TVSerie()
        {
            Name = name,
            Overview = overview,
            PosterPath = posterPath,
            BackdropPath = backdropPath,
            CountryIso = countryIso,
            FirstAirDate = firstAirDate?.ToUniversalTime(),
            LastAirDate = lastAirDate?.ToUniversalTime()
        };
        tvSerie.AddDomainEvent(new TVSerieCreatedEvent(tvSerie));
        return tvSerie;
    }

    public void AssignKeywords(IEnumerable<Keyword> keywords)
    {
        if (keywords == null) return;
        Keywords.Clear();
        foreach (var keyword in keywords)
        {
            Keywords.Add(keyword);
        }
    }

    public void AssignGenres(IEnumerable<Genre> genres)
    {
        if (genres == null) return;
        Genres.Clear();
        foreach (var genre in genres)
        {
            Genres.Add(genre);
        }
    }

    public void Update(string? name, string? overview, string? posterPath,
        string? backdropPath, DateTime? firstAirDate, DateTime? lastAirDate, string? countryIso)
    {
        Name = name ?? Name;
        Overview = overview ?? Overview;
        PosterPath = posterPath ?? PosterPath;
        BackdropPath = backdropPath ?? BackdropPath;
        CountryIso = countryIso ?? CountryIso;
        FirstAirDate = firstAirDate?.ToUniversalTime() ?? FirstAirDate;
        LastAirDate = lastAirDate?.ToUniversalTime() ?? LastAirDate;
    }
}