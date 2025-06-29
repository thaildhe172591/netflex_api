namespace Netflex.Domain.Entities;

public class Actor : Entity<long>
{
    public required string Name { get; set; }
    public string? Image { get; set; }
    public bool Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Biography { get; set; }
    public static Actor Create(string name, bool gender, DateTime? birthDate, string? biography, string? image)
    {
        var actor = new Actor()
        {
            Name = name,
            Gender = gender,
            BirthDate = birthDate?.ToUniversalTime(),
            Biography = biography,
            Image = image
        };
        return actor;
    }

    public void Update(string? name, string? image, bool? gender, DateTime? birthDate, string? biography)
    {
        Name = name ?? Name;
        Image = image ?? Image;
        Gender = gender ?? Gender;
        BirthDate = birthDate?.ToUniversalTime() ?? BirthDate;
        Biography = biography ?? Biography;
    }
}