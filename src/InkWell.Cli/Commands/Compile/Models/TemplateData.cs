namespace InkWell.Cli.Commands.Compile.Models;

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
