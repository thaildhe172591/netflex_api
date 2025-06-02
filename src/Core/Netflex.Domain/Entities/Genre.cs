namespace Netflex.Domain.Entities;

public class Genre : Entity<long>
{
    public required string Name { get; set; }
    private Genre() { }
    public static Genre Create(string name)
    {
        var genre = new Genre()
        {
            Name = name
        };
        return genre;
    }
}