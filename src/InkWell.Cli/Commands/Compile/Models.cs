namespace InkWell.Cli.Commands.Compile;

public record FrontMatter(
    string Title, // only title is required
    string? Desc,
    string? Updated,
    string? Created,
    string? Published,
    bool? IsDraft,
    string? Summary,
    AuthorInfo? Author)
{
    public FrontMatter() : this(
        Title: string.Empty,
        Desc: null,
        Updated: null,
        Created: null,
        Published: null,
        IsDraft: true,
        Summary: null,
        Author: null)
    { } // for YamlDotNet
}

public record AuthorInfo(string Name, string Email)
{
    public AuthorInfo() : this(
        string.Empty,
        string.Empty)
    { } // for YamlDotNet
}

public record MarkdownContent(
    FrontMatter Info,
    string Family,
    string Html
);
