using InkWell.Cli.Tools;

namespace InkWell.Cli.Core;

public interface IContentInfo
{
    Task<string[]> GetAllMarkdownPaths(string sourcePath);
}

public class SiteInfo(IDirectoryTool directory) : IContentInfo
{
    public Task<string[]> GetAllMarkdownPaths(string sourcePath)
    {
        var contentDir = sourcePath.Path("content");
        var contents = directory.GetFiles(contentDir, "*.md", SearchOption.AllDirectories);

        return Task.FromResult(contents);
    }
}
