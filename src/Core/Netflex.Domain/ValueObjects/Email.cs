using System.Text.RegularExpressions;

namespace Netflex.Domain.ValueObjects;

public partial record Email
{
    public string Value { get; }
    private Email(string value) => Value = value;

    public static Email Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(Email));

        if (!EmailRegex().IsMatch(value))
            throw new InvalidFormatException(nameof(PhoneNumber));

        return new Email(value);
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex EmailRegex();
}