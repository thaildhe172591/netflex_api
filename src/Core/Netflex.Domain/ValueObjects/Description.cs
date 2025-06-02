namespace Netflex.Domain.ValueObjects;

public sealed record Description
{
    public const int MAX_LENGTH = 500;

    public string Value { get; }
    private Description(string value) => Value = value;

    public static Description Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(Description));
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value.Length, MAX_LENGTH, nameof(Description));
        return new Description(value);
    }
}