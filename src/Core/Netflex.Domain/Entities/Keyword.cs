namespace Netflex.Domain.Entities;

public class Keyword : Entity<long>
{
    public required string Name { get; set; }
    private Keyword() { }
    public static Keyword Create(string name)
    {
        var keyword = new Keyword()
        {
            Name = name
        };
        return keyword;
    }
}