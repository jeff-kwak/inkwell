namespace InkWell.Cli.Tools;

public interface IFileTool
{
    Task<string> ReadAllTextAsync(params string[] paths);
}

public class FileTool : IFileTool
{
    public async Task<string> ReadAllTextAsync(params string[] paths)
    {
        var fullPath = Path.Combine(paths);
        return await File.ReadAllTextAsync(fullPath);
    }
}
