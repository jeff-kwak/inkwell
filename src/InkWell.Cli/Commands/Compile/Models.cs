namespace InkWell.Cli.Commands.Compile;

public record ContentInfo(string Title)
{
    public ContentInfo() : this(string.Empty) { } // for YamlDotNet
}

public record MarkdownContent(
    ContentInfo Info,
    string Family,
    string Html
);
