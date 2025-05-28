namespace InkWell.Cli.Tools;

public static class PathExtensions
{
    public static string Path(this string path, params string[] paths) =>
        System.IO.Path.Combine([path, .. paths]);
}
