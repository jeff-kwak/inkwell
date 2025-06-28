namespace InkWell.Cli.Commands.Compile.Models;

public record FrontMatter(
    string Title, // only title is required
    string? Desc,
    bool? FileNameMatchesTitle,
    DateTime? Updated,
    DateTime? Created,
    DateTime? Published,
    string? Summary,
    AuthorInfo? Author)
{
    public FrontMatter() : this(
        Title: string.Empty,
        Desc: null,
        FileNameMatchesTitle: true,
        Updated: null,
        Created: null,
        Published: null,
        Summary: null,
        Author: null)
    { } // for YamlDotNet
}
