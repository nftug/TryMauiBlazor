namespace TryMauiBlazor.Extensions;

internal static class StringExtension
{
    public static string? GetFirstLine(this string self)
    {
        var separator = new[] { Environment.NewLine };

        return self
            .Split(separator, StringSplitOptions.None)
            .FirstOrDefault();
    }
}

