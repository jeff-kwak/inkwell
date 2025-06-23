namespace InkWell.Cli.Commands.Compile;

public record FrontMatter(
    string Title, // only title is required
    string? Desc,
    DateTime? Updated,
    DateTime? Created,
    DateTime? Published,
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

// Template data is passed to the template renderer.
// Many of the fields have been "pre-processed" and formatted.
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
            Updated: content.Info.Updated.Iso8601(),
            Created: content.Info.Created.Iso8601(),
            Published: content.Info.Published.Iso8601(),
            Summary: content.Info.Summary,
            Author: content.Info.Author,
            Html: content.Html
        );
    }
}

public static class TemplateDataExtensions
{
    public static string? Iso8601(this DateTime? dateTime)
    {
        return dateTime?.ToString("yyyy-MM-ddTHH:mm:ss");
    }
}
