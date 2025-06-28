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

    public static string DirName(this string path)
    {
        return System.IO.Path.GetDirectoryName(path) ?? string.Empty;
    }

    public static string RemovePathBefore(this string path, string family)
    {
        var pathStart = path.IndexOf(family, StringComparison.OrdinalIgnoreCase);
        if (pathStart < 0)
        {
            return path; // If family not found, return original path
        }
        return path.Substring(pathStart).TrimStart(System.IO.Path.DirectorySeparatorChar);
    }

    public static string FileNameWithoutExtension(this string path)
    {
        var fileName = System.IO.Path.GetFileNameWithoutExtension(path);
        return fileName ?? string.Empty; // Return empty if no file name
    }
}
