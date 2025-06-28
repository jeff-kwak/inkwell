namespace InkWell.Cli.Commands.Compile.Models;

public record MarkdownContent(
    FrontMatter Info,
    string Path,
    string Family,
    string Html
)
{
    public bool IsRoot()
    {
        return Family.Equals("root", StringComparison.OrdinalIgnoreCase);
    }
}
