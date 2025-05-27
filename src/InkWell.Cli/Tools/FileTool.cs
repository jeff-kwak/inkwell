namespace InkWell.Cli.Tools;

public interface IFileTool
{
    void Copy(string source, string destination) => File.Copy(source, destination);
    bool Exists(string v) => File.Exists(v);
    string ReadAllText(string path) => File.ReadAllText(path);
}

public class FileTool : IFileTool
{
}
