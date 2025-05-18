namespace InkWell.Cli.Tools;

public interface IDirectoryTool
{
    bool Exists(string path);
}

public class DirectoryTool : IDirectoryTool
{
    public bool Exists(string path) => Directory.Exists(path);
}
