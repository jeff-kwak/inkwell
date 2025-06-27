using System.Text.RegularExpressions;

namespace InkWell.Cli.Tools;

public static partial class PathExtensions
{
    public static string Path(this string path, params string[] paths) =>
        System.IO.Path.Combine([path, .. paths]);

    public static string ToSafeFileName(this string title)
    {
        var lower = title.ToLowerInvariant();
        var safe = NotAllowed().Replace(lower, "-");// Remove special characters
        var deduped = MultipleDashes().Replace(safe, "-"); // Replace multiple consecutive dashes with a single dash
        return deduped.Trim('-');
    }

    [GeneratedRegex(@"[^\w\.]")]
    private static partial Regex NotAllowed();

    [GeneratedRegex(@"-+")]
    private static partial Regex MultipleDashes();
}
