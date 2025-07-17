namespace Netflex.Domain.ValueObjects;

public record Rating
{
    public int Value { get; }
    private Rating(int value)
    {
        Value = value;
    }

    public static Rating Of(int value)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 5, nameof(Rating));
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 0, nameof(Rating));
        return new Rating(value);
    }
}