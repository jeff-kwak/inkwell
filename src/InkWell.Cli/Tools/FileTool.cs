namespace InkWell.Cli.Tools;

public interface IFileTool
{
    void Copy(string source, string destination) => File.Copy(source, destination);
}

public class FileTool : IFileTool
{
}
