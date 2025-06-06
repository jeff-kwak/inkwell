namespace InkWell.Cli.Tools;

public interface IFileTool
{
    Task<string> ReadAllTextAsync(params string[] paths);
    Task WriteAllTextAsync(string content, params string[] paths);
}

public class FileTool : IFileTool
{
    public async Task<string> ReadAllTextAsync(params string[] paths)
    {
        var fullPath = Path.Combine(paths);
        return await File.ReadAllTextAsync(fullPath);
    }

    public async Task WriteAllTextAsync(string content, params string[] paths)
    {
        var fullPath = Path.Combine(paths);
        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        await File.WriteAllTextAsync(fullPath, content);
    }
}
