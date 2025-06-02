namespace Netflex.Domain.ValueObjects;

public sealed record LoginProvider
{
    public static readonly LoginProvider Google = new("google");
    public string Value { get; }
    private LoginProvider(string value) => Value = value.ToLowerInvariant();

    public static LoginProvider Of(string value)
    {
        return value.ToLowerInvariant() switch
        {
            "google" => Google,
            _ => throw new NotSupportedException(nameof(LoginProvider))
        };
    }
}