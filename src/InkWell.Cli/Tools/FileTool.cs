namespace InkWell.Cli.Tools;

public interface IFileTool
{
    void Copy(string source, string destination) => File.Copy(source, destination);
    bool Exists(string v) => File.Exists(v);
}

public class FileTool : IFileTool
{
}
