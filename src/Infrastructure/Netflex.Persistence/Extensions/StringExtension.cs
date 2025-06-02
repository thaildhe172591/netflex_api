namespace Netflex.Persistence.Extensions;

public static class StringExtension
{
    public static string ToSnakeCase(this string str) =>
        string.Concat(str.Select((x, i) =>
            i > 0 && char.IsUpper(x) && (char.IsLower(str[i - 1]) || i < str.Length - 1 && char.IsLower(str[i + 1]))
                ? "_" + x
                : x.ToString())).ToLowerInvariant();

    public static string Pluralize(this string str)
    {
        var exceptions = new Dictionary<string, string>() {
                { "man", "men" },
                { "woman", "women" },
                { "child", "children" },
                { "tooth", "teeth" },
                { "foot", "feet" },
                { "mouse", "mice" },
                { "belief", "beliefs" } };

        if (exceptions.ContainsKey(str.ToLowerInvariant()))
        {
            return exceptions[str.ToLowerInvariant()];
        }
        else if (str.EndsWith("y", StringComparison.OrdinalIgnoreCase) &&
            !str.EndsWith("ay", StringComparison.OrdinalIgnoreCase) &&
            !str.EndsWith("ey", StringComparison.OrdinalIgnoreCase) &&
            !str.EndsWith("iy", StringComparison.OrdinalIgnoreCase) &&
            !str.EndsWith("oy", StringComparison.OrdinalIgnoreCase) &&
            !str.EndsWith("uy", StringComparison.OrdinalIgnoreCase))
        {
            return str[..^1] + "ies";
        }
        else if (str.EndsWith("us", StringComparison.InvariantCultureIgnoreCase))
        {
            return str + "es";
        }
        else if (str.EndsWith("ss", StringComparison.InvariantCultureIgnoreCase))
        {
            return str + "es";
        }
        else if (str.EndsWith("s", StringComparison.InvariantCultureIgnoreCase))
        {
            return str;
        }
        else if (str.EndsWith("x", StringComparison.InvariantCultureIgnoreCase) ||
            str.EndsWith("ch", StringComparison.InvariantCultureIgnoreCase) ||
            str.EndsWith("sh", StringComparison.InvariantCultureIgnoreCase))
        {
            return str + "es";
        }
        else if (str.EndsWith("f", StringComparison.InvariantCultureIgnoreCase) && str.Length > 1)
        {
            return str[..^1] + "ves";
        }
        else if (str.EndsWith("fe", StringComparison.InvariantCultureIgnoreCase) && str.Length > 2)
        {
            return str[..^2] + "ves";
        }
        else
        {
            return str + "s";
        }
    }
}