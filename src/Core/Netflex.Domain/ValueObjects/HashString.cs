namespace Netflex.Domain.ValueObjects;

public sealed record HashString
{
    public string Value { get; }
    public HashString(string value)
    {
        Value = value;
    }
    public static HashString Of(string str)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(str, nameof(str));
        return new HashString(BCrypt.Net.BCrypt.HashPassword(str));
    }

    public bool Verify(string str) => BCrypt.Net.BCrypt.Verify(str, Value);
}