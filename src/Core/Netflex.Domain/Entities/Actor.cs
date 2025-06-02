namespace Netflex.Domain.Entities;

public class Actor : Entity<long>
{
    public required string Name { get; set; }
    public string? Image { get; set; }
    public bool Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Biography { get; set; }
    private Actor() { }
    public static Actor Create(string name, bool gender, DateTime? birthDate, string? biography, string? image)
    {
        var actor = new Actor()
        {
            Name = name,
            Gender = gender,
            BirthDate = birthDate,
            Biography = biography,
            Image = image
        };
        return actor;
    }
}