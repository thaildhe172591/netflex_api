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
    private Movie() { }
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

}