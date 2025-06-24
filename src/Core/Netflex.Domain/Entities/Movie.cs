namespace Netflex.Domain.Entities;

public class Movie : Aggregate<long>
{
    public required string Title { get; set; }
    public string? Overview { get; set; }
    public string? PosterPath { get; set; }
    public string? BackdropPath { get; set; }
    public string? VideoURL { get; set; }
    public string? CountryISO { get; set; }
    public TimeSpan? RunTime { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public virtual ICollection<Actor> Actors { get; set; } = [];
    public virtual ICollection<Keyword> Keywords { get; set; } = [];
    public virtual ICollection<Genre> Genres { get; set; } = [];
    public static Movie Create(string title, string? overview = default, string? posterPath = default,
        string? backdropPath = default, TimeSpan? runTime = default, DateTime? releaseDate = default,
            string? countryISO = default, string? videoURL = default)
    {
        var movie = new Movie()
        {
            Title = title,
            RunTime = runTime,
            ReleaseDate = releaseDate,
            Overview = overview,
            PosterPath = posterPath,
            BackdropPath = backdropPath,
            VideoURL = videoURL,
            CountryISO = countryISO
        };
        movie.AddDomainEvent(new MovieCreatedEvent(movie));
        return movie;
    }

    public void Update(string? title, string? overview, string? posterPath, string? backdropPath,
        TimeSpan? runTime, DateTime? releaseDate, string? countryISO, string? videoURL)
    {
        Title = title ?? Title;
        Overview = overview ?? Overview;
        PosterPath = posterPath ?? PosterPath;
        BackdropPath = backdropPath ?? BackdropPath;
        VideoURL = videoURL ?? VideoURL;
        CountryISO = countryISO ?? CountryISO;
        RunTime = runTime ?? RunTime;
        ReleaseDate = releaseDate ?? ReleaseDate;
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