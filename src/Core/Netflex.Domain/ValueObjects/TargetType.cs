namespace Netflex.Domain.ValueObjects;

public record TargetType
{
    public static readonly TargetType Movie = new("movie");
    public static readonly TargetType Episode = new("episode");
    public static readonly TargetType TVSerie = new("serie");
    public string Value { get; }
    private TargetType(string value)
    {
        Value = value.ToLowerInvariant();
    }

    public static TargetType Of(string value)
    {
        return value.ToLowerInvariant() switch
        {
            "movie" => Movie,
            "episode" => Episode,
            "serie" => TVSerie,
            _ => throw new ArgumentException($"Invalid target type: {value}")
        };
    }
}