namespace InkWell.Cli.Commands.Compile;

public record FrontMatter(
    string Title, // only title is required
    string? Desc,
    string? Updated,
    string? Created,
    string? Published,
    string? Summary,
    AuthorInfo? Author)
{
    public FrontMatter() : this(
        Title: string.Empty,
        Desc: null,
        Updated: null,
        Created: null,
        Published: null,
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

public record TemplateData(
    string Title,
    string? Desc,
    string? Updated,
    string? Created,
    string? Published,
    string? Summary,
    AuthorInfo? Author,
    string Html)
{
    public static TemplateData FromMarkdownContent(MarkdownContent content)
    {
        return new TemplateData(
            Title: content.Info.Title,
            Desc: content.Info.Desc,
            Updated: content.Info.Updated,
            Created: content.Info.Created,
            Published: content.Info.Published,
            Summary: content.Info.Summary,
            Author: content.Info.Author,
            Html: content.Html
        );
    }
}
