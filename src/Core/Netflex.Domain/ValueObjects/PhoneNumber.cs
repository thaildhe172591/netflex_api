using System.Text.RegularExpressions;

namespace Netflex.Domain.ValueObjects;

public partial record PhoneNumber
{
    public string Value { get; }
    private PhoneNumber(string value) => Value = value;

    public static PhoneNumber Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(PhoneNumber));

        if (!PhoneRegex().IsMatch(value))
            throw new InvalidFormatException(nameof(PhoneNumber));

        return new PhoneNumber(value);
    }

    [GeneratedRegex(@"^\+?[1-9]\d{1,14}$")]
    private static partial Regex PhoneRegex();
}