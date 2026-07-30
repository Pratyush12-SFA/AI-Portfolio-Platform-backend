using System.Globalization;

namespace AIPortfolio.Application.Helpers;

public static class DateParser
{
    private static readonly string[] Formats =
    [
        "yyyy-MM-dd",
        "yyyy-MM",
        "yyyy",
        "MMM yyyy",
        "MMMM yyyy",
        "MM/dd/yyyy",
        "dd/MM/yyyy",
        "M/d/yyyy",
        "d/M/yyyy"
    ];

    public static DateTime ParseDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return DateTime.MinValue;

        if (DateTime.TryParseExact(value.Trim(), Formats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var result))
            return result;

        if (DateTime.TryParse(value.Trim(), CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var fallback))
            return fallback;

        return DateTime.MinValue;
    }

    public static DateTime? ParseNullableDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (DateTime.TryParseExact(value.Trim(), Formats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var result))
            return result;

        if (DateTime.TryParse(value.Trim(), CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var fallback))
            return fallback;

        return null;
    }
}