namespace Netflex.Domain.Entities;

public class Movie : Aggregate<long>
{
    public required string Title { get; set; }
    public string? Overview { get; set; }
    public string? PosterPath { get; set; }
    public string? BackdropPath { get; set; }
    public string? VideoUrl { get; set; }
    public string? CountryIso { get; set; }
    public TimeSpan? RunTime { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public virtual ICollection<Actor> Actors { get; set; } = [];
    public virtual ICollection<Keyword> Keywords { get; set; } = [];
    public virtual ICollection<Genre> Genres { get; set; } = [];
    public static Movie Create(string title, string? overview = default, string? posterPath = default,
        string? backdropPath = default, TimeSpan? runTime = default, DateTime? releaseDate = default,
            string? countryIso = default, string? videoUrl = default)
    {
        var movie = new Movie()
        {
            Title = title,
            RunTime = runTime,
            ReleaseDate = releaseDate?.ToUniversalTime(),
            Overview = overview,
            PosterPath = posterPath,
            BackdropPath = backdropPath,
            VideoUrl = videoUrl,
            CountryIso = countryIso
        };
        movie.AddDomainEvent(new MovieCreatedEvent(movie));
        return movie;
    }

    public void Update(string? title, string? overview, string? posterPath, string? backdropPath,
        TimeSpan? runTime, DateTime? releaseDate, string? countryIso, string? videoUrl)
    {
        Title = title ?? Title;
        Overview = overview ?? Overview;
        PosterPath = posterPath ?? PosterPath;
        BackdropPath = backdropPath ?? BackdropPath;
        VideoUrl = videoUrl ?? VideoUrl;
        CountryIso = countryIso ?? CountryIso;
        RunTime = runTime ?? RunTime;
        ReleaseDate = releaseDate?.ToUniversalTime() ?? ReleaseDate;
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
}