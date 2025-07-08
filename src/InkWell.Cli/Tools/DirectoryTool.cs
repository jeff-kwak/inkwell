namespace InkWell.Cli.Tools;

public interface IDirectoryTool
{
    string[] GetDirectories(
        string path,
        string searchPattern = "*",
        SearchOption searchOption = SearchOption.TopDirectoryOnly);

    string[] GetFiles(
        string path,
        string searchPattern = "*",
        SearchOption searchOption = SearchOption.TopDirectoryOnly);

    void EnsureDirectory(string path);

    void CleanDirectory(string path);

    void CopyDirectory(string source, string destination);
}

public class DirectoryTool : IDirectoryTool
{
    public string[] GetDirectories(
        string path,
        string searchPattern = "*",
        SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        return Directory.GetDirectories(path, searchPattern, searchOption);
    }

    public string[] GetFiles(
        string path,
        string searchPattern = "*",
        SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        return Directory.GetFiles(path, searchPattern, searchOption);
    }

    public string GetDirectoryName(string path)
    {
        return Path.GetDirectoryName(path) ?? string.Empty;
    }

    public void EnsureDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public void CleanDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        Directory.CreateDirectory(path);
    }

    public void CopyDirectory(string source, string destination)
    {
        EnsureDirectory(destination);
        foreach (var file in Directory.GetFiles(source))
        {
            var destFile = Path.Combine(destination, Path.GetFileName(file));
            File.Copy(file, destFile, true);

            foreach (var dir in Directory.GetDirectories(destination))
            {
                var destDir = Path.Combine(destination, Path.GetFileName(dir));
                CopyDirectory(dir, destDir);
            }
        }
    }
}
