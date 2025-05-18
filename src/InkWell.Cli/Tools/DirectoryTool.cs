namespace InkWell.Cli.Tools;

public interface IDirectoryTool
{
    bool Exists(string path) => Directory.Exists(path);
    IEnumerable<string> GetFiles(string outputPath, string searchPath, SearchOption searchOptions) =>
        Directory.GetFiles(outputPath, searchPath, searchOptions);

    IEnumerable<string> GetDirectories(string outputPath, string searchPath, SearchOption searchOptions) =>
        Directory.GetDirectories(outputPath, searchPath, searchOptions);

    void CreateDirectory(string path) => Directory.CreateDirectory(path);
    void DeleteFile(string filePath) => File.Delete(filePath);
    void DeleteDirectory(string directoryPath) => Directory.Delete(directoryPath, true);

    void CopyDirectory(string sourceDir, string destDir, bool recursive = true);
}

public class DirectoryTool(IFileTool fileTool) : IDirectoryTool
{
    public void CopyDirectory(string sourceDir, string destDir, bool recursive = true)
    {
        if (!Directory.Exists(sourceDir))
            throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");

        Directory.CreateDirectory(destDir);

        foreach (string file in Directory.GetFiles(sourceDir))
        {
            string targetFilePath = Path.Combine(destDir, Path.GetFileName(file));
            fileTool.Copy(file, targetFilePath);
        }

        if (recursive)
        {
            foreach (var subDir in Directory.GetDirectories(sourceDir))
            {
                string newDestinationDir = Path.Combine(destDir, Path.GetFileName(subDir));
                CopyDirectory(subDir, newDestinationDir, true);
            }
        }
    }
}
